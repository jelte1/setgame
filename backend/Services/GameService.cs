using AutoMapper;
using backend.DTOs.Card;
using backend.DTOs.Game;
using backend.Dtos.GameState;
using backend.Entities;
using backend.Interfaces;

namespace backend.Services;

public class GameService : IGameService
{
    private readonly IGamesRepository _gamesRepository;
    private readonly ICardsRepository _cardsRepository;
    private readonly IGameStatesRepository _gameStatesRepository;
    private readonly IFoundSetsRepository _foundSetRepository;
    private readonly ISetValidationService _setValidationService;
    private readonly IMapper _mapper;
    
    public GameService(IGamesRepository gamesRepository, IGameStatesRepository gameStatesRepository, ICardsRepository cardsRepository, IFoundSetsRepository foundSetsRepository, ISetValidationService setValidationService, IMapper mapper)
    {
        _gamesRepository = gamesRepository;
        _cardsRepository = cardsRepository;
        _gameStatesRepository = gameStatesRepository;
        _foundSetRepository = foundSetsRepository;
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

        // Temp: fixed card order for testing — lots of sets available
        var fixedTableIds = new List<int> { 19, 23, 27, 1, 43, 76, 13, 15, 20, 8, 31, 54 };
        var tableCards = fixedTableIds
            .Select(id => cards.First(c => c.Id == id))
            .ToList();
        var remainingCards = cards
            .Where(c => !fixedTableIds.Contains(c.Id))
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        var orderedCards = tableCards.Concat(remainingCards).ToList();

        var gameStates = new List<GameState>();
        for (int i = 0; i < orderedCards.Count; i++)
        {
            gameStates.Add(new GameState
            {
                GameId = game.Id,
                CardId = orderedCards[i].Id,
                Location = i < 12 ? CardLocation.Table : CardLocation.Deck,
                Order = i
            });
        }

        await _gameStatesRepository.AddRangeAsync(gameStates);
        await _gameStatesRepository.SaveChangesAsync();

        return game;
    }
    // public async Task<Game> CreateGameAsync(string userId)
    // {
    //     var game = new Game
    //     {
    //         UserId = userId,
    //         CreatedAt = DateTime.Now
    //     };
    //     await _gamesRepository.AddAsync(game);
    //     await _gamesRepository.SaveChangesAsync();
    //     
    //     var cards = await _cardsRepository.GetAllAsync();
    //     
    //     var shuffled = cards.OrderBy(_ => Guid.NewGuid()).ToList();
    //     
    //     var gameStates = new List<GameState>();
    //     for (int i = 0; i < shuffled.Count; i++)
    //     {
    //         var gameState = new GameState
    //         {
    //             GameId = game.Id,
    //             CardId = shuffled[i].Id,
    //             Location = CardLocation.Deck,
    //             Order = i
    //         };
    //         await _gameStatesRepository.AddAsync(gameState);
    //         gameStates.Add(gameState);
    //     }
    //     await _gameStatesRepository.SaveChangesAsync();
    //     
    //     var first12 = gameStates
    //         .OrderBy(gs => gs.Order)
    //         .Take(12)
    //         .ToList();
    //
    //     foreach (var gs in first12)
    //     {
    //         gs.Location = CardLocation.Table;
    //         await _gameStatesRepository.UpdateAsync(gs);
    //     }
    //     
    //     await _gameStatesRepository.SaveChangesAsync();
    //
    //     return game;
    // }
    
    public async Task<CheckSetResponseDto> CheckSet(int gameId, CheckSetDto checkSetDto)
    {
        var game = await _gamesRepository.GetGameWithStatesAsync(gameId);
        
        if (game == null)
        {
            return null;
        }
        
        var cardList = checkSetDto.ToList();
        
        var cardStates = game.GameStates
            .Where(gs => cardList.Contains(gs.CardId) && gs.Location == CardLocation.Table)
            .Select(gs => gs.Card)
            .ToList();
        
        if (cardStates.Count != 3)
        {
            return new CheckSetResponseDto
            {
                IsSet = false
            };
        }
        
        var cards = await _cardsRepository.GetCardsByIds(cardStates.Select(c => c.Id).ToList());
        
        if (cards.Count != 3)
        {
            return new CheckSetResponseDto
            {
                IsSet = false
            };
        }
        
        bool isSet = _setValidationService.IsValidSet(cards[0], cards[1], cards[2]);
        
        if (!isSet)
        {
            return new CheckSetResponseDto
            {
                IsSet = false
            };
        }
        
        // discard the cards
        foreach (var gs in game.GameStates.Where(gs => cardList.Contains(gs.CardId)))
        {
            gs.Location = CardLocation.Discarded;
            await _gameStatesRepository.UpdateAsync(gs); //??????????????????????????????????????????????????
        }
        await _gameStatesRepository.SaveChangesAsync();
        
        // save the found set
        var foundSet = new FoundSet()
        {
            GameId = game.Id,
            Card1Id = cards[0].Id,
            Card2Id = cards[1].Id,
            Card3Id = cards[2].Id
        };
        
        await _foundSetRepository.AddAsync(foundSet);
        await _foundSetRepository.SaveChangesAsync();
        
        var newGameStates = await DrawCards(game);

        return new CheckSetResponseDto
        {
            IsSet = true,
            NewGameStates = _mapper.Map<List<GetGameStateDto>>(newGameStates)
        };

    }
    
    public async Task<List<GameState>> DrawCards(Game game)
    {
        var cards = game.GameStates
            .Where(gs => gs.Location == CardLocation.Deck)
            .OrderBy(gs => gs.Order)
            .Take(3)
            .ToList();

        foreach (var gs in cards)
        {
            gs.Location = CardLocation.Table;
            await _gameStatesRepository.UpdateAsync(gs);
        }
        await _gameStatesRepository.SaveChangesAsync();
        
        return cards; 
    }
}