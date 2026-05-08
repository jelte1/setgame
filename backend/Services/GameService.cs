using AutoMapper;
using backend.DTOs.Card;
using backend.Dtos.FoundSet;
using backend.DTOs.Game;
using backend.Dtos.GameCardState;
using backend.Entities;
using backend.Interfaces;

namespace backend.Services;

public class GameService : IGameService
{
    private readonly IGamesRepository _gamesRepository;
    private readonly ICardsRepository _cardsRepository;
    private readonly IGameCardStatesRepository _gameCardStatesRepository;
    private readonly IFoundSetsRepository _foundSetsRepository;
    private readonly ISetValidationService _setValidationService;
    private readonly IMapper _mapper;

    public GameService(
        IGamesRepository gamesRepository,
        IGameCardStatesRepository gameCardStatesRepository,
        ICardsRepository cardsRepository,
        IFoundSetsRepository foundSetsesRepository,
        ISetValidationService setValidationService,
        IMapper mapper)
    {
        _gamesRepository = gamesRepository;
        _cardsRepository = cardsRepository;
        _gameCardStatesRepository = gameCardStatesRepository;
        _foundSetsRepository = foundSetsesRepository;
        _setValidationService = setValidationService;
        _mapper = mapper;
    }

    public async Task<Game> CreateGameAsync(string userId)
    {
        var game = new Game
        {
            UserId = userId,
            CreatedAt = DateTime.Now
        };
        await _gamesRepository.AddAsync(game);
        await _gamesRepository.SaveChangesAsync();

        var cards = await _cardsRepository.GetAllAsync();
        var shuffled = cards.OrderBy(_ => Guid.NewGuid()).ToList();
        var gameCardStates = new List<GameCardState>();

        for (int i = 0; i < shuffled.Count; i++)
        {
            var gameState = new GameCardState
            {
                GameId = game.Id,
                CardId = shuffled[i].Id,
                Location = CardLocation.Deck,
                Order = i
            };
            await _gameCardStatesRepository.AddAsync(gameState);
            gameCardStates.Add(gameState);
        }

        await _gameCardStatesRepository.SaveChangesAsync();

        await DrawCards(game, 12);
        await EnsureValidTable(game);

        return game;
    }
    
    private async Task EnsureValidTable(Game game)
    {
        while (true)
        {
            var tableCards = GetGameCardStatesByLocation(game, CardLocation.Table)
                .Select(gs => gs.Card)
                .ToList();

            if (_setValidationService.FindAllSets(tableCards).Count > 0)
            {
                return;
            }

            var allDeckCards = GetGameCardStatesByLocation(game, CardLocation.Deck);
            var shuffled = allDeckCards.OrderBy(_ => Random.Shared.Next()).ToList();
            for (int i = 0; i < shuffled.Count; i++)
            {
                shuffled[i].Order = i;
                await _gameCardStatesRepository.UpdateAsync(shuffled[i]);
            }
            await _gameCardStatesRepository.SaveChangesAsync();

            // Move all table cards back to deck
            var tableStates = GetGameCardStatesByLocation(game, CardLocation.Table);
            foreach (var gs in tableStates)
            {
                gs.Location = CardLocation.Deck;
                await _gameCardStatesRepository.UpdateAsync(gs);
            }
            await _gameCardStatesRepository.SaveChangesAsync();

            await DrawCards(game, 12);
        }
    }
    
    public async Task<CheckSetResponseDto> CheckSet(int gameId, CheckSetDto checkSetDto)
    {
        var game = await _gamesRepository.GetGameWithStatesAsync(gameId);
        
        if (game == null)
        {
            return null;
        }
        
        var cardList = checkSetDto.ToList();
        
        var cardStates = game.GameCardStates
            .Where(gs => cardList.Contains(gs.CardId) && gs.Location == CardLocation.Table)
            .Select(gs => gs.Card)
            .ToList();
        
        // if there arent 3 cardstates and/or not on the table
        if (cardStates.Count != 3)
        {
            return new CheckSetResponseDto
            {
                IsSet = false
            };
        }
        
        var cards = await _cardsRepository.GetCardsByIds(cardStates.Select(c => c.Id).ToList());
        
        // if the cards exist (almost redundant check)
        if (cards.Count != 3)
        {
            return new CheckSetResponseDto
            {
                IsSet = false
            };
        }
        
        var isSet = _setValidationService.IsValidSet(cards[0], cards[1], cards[2]);
        var existsFoundSet = await _foundSetsRepository.ExistsSet(gameId, cards[0].Id, cards[1].Id, cards[2].Id);
        
        // check if card combination is a set or if the set has already been found
        if (!isSet || existsFoundSet)
        {
            return new CheckSetResponseDto
            {
                IsSet = false
            };
        }
        
        // discard the foundset cards
        foreach (var gs in game.GameCardStates.Where(gs => cardList.Contains(gs.CardId)))
        {
            gs.Location = CardLocation.Discarded;
            await _gameCardStatesRepository.UpdateAsync(gs);
        }
        await _gameCardStatesRepository.SaveChangesAsync();
        
        // save the found set
        var foundSet = new FoundSet()
        {
            GameId = game.Id,
            Card1Id = cards[0].Id,
            Card2Id = cards[1].Id,
            Card3Id = cards[2].Id
        };
        
        await _foundSetsRepository.AddAsync(foundSet);
        await _foundSetsRepository.SaveChangesAsync();
        
        await EnsureValidDraw(game);
        
        var finalTableCards = GetGameCardStatesByLocation(game, CardLocation.Table)
            .Select(gs => gs.Card)
            .ToList();

        var possibleSetsCount = _setValidationService.FindAllSets(finalTableCards).Count;
        
        if (possibleSetsCount == 0 && !GetGameCardStatesByLocation(game, CardLocation.Deck).Any())
        {
            game.IsFinished = true;
            await _gamesRepository.UpdateAsync(game);
            await _gamesRepository.SaveChangesAsync();
        }
        
        return new CheckSetResponseDto
        {
            IsSet = true,
            NewGameCardStates = _mapper.Map<List<GetGameCardStateDto>>(GetGameCardStatesByLocation(game, CardLocation.Table)),
            FoundSet = _mapper.Map<GetFoundSetDto>(foundSet),
            PossibleSetsCount = possibleSetsCount,
            IsFinished = game.IsFinished
        };

    }

    private async Task EnsureValidDraw(Game game)
    {
        // Draw cards until the table has at least one valid set
        var random = new Random();
        var newGameCardStates = new List<GameCardState>();

        while (true)
        {
            newGameCardStates = await DrawCards(game);

            var newTableCards = GetGameCardStatesByLocation(game, CardLocation.Table)
                .Select(gs => gs.Card)
                .ToList();

            // set found; game can continue
            if (_setValidationService.FindAllSets(newTableCards).Count > 0)
            {
                break;
            }

            // no more cards to draw from deck; exit
            if (!GetGameCardStatesByLocation(game, CardLocation.Deck).Any())
            {
                break;
            }
            
            // if no valid set then reshuffle the 3 drawn cards back into the deck
            foreach (var gs in newGameCardStates)
            {
                gs.Location = CardLocation.Deck;
            }

            // since no set was found; reshuffle the deck by randomizing the order of the cards in the deck
            var allDeckCards = GetGameCardStatesByLocation(game, CardLocation.Deck);
            var shuffledOrders = allDeckCards.Select(gs => gs.Order)
                .OrderBy(_ => random.Next())
                .ToList();

            for (int i = 0; i < allDeckCards.Count; i++)
            {
                allDeckCards[i].Order = shuffledOrders[i];
                await _gameCardStatesRepository.UpdateAsync(allDeckCards[i]);
            }
            await _gameCardStatesRepository.SaveChangesAsync();
        }

    }

    public async Task<List<GameCardState>> DrawCards(Game game, int amount = 3)
    {
        var gameCardStates = GetGameCardStatesByLocation(game, CardLocation.Deck, amount);

        foreach (var gs in gameCardStates)
        {
            gs.Location = CardLocation.Table;
            await _gameCardStatesRepository.UpdateAsync(gs);
        }

        await _gameCardStatesRepository.SaveChangesAsync();

        return gameCardStates;
    }

    public (Card, Card, Card)? GetHint(Game game)
    {
        var tableCards = GetGameCardStatesByLocation(game, CardLocation.Table)
            .Select(gs => gs.Card)
            .ToList();
        
        return _setValidationService.FindHint(tableCards);
    }

    private List<GameCardState> GetGameCardStatesByLocation(Game game, CardLocation location, int? amount = null)
    {
        var gameCardStates = game.GameCardStates
            .Where(gs => gs.Location == location)
            .OrderBy(gs => gs.Order);

        if (amount.HasValue)
        {
            return gameCardStates.Take(amount.Value).ToList();
        }
        
        return gameCardStates.ToList();
    }
}