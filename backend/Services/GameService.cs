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

    public GameService(IGamesRepository gamesRepository, IGameCardStatesRepository gameCardStatesRepository, ICardsRepository cardsRepository, IFoundSetsRepository foundSetsesRepository, ISetValidationService setValidationService, IMapper mapper)
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

        var first12 = gameCardStates
            .OrderBy(gs => gs.Order)
            .Take(12)
            .ToList();

        foreach (var gs in first12)
        {
            gs.Location = CardLocation.Table;
            await _gameCardStatesRepository.UpdateAsync(gs);
        }

        await _gameCardStatesRepository.SaveChangesAsync();

        await EnsureValidTable(game);

        return game;
    }

    public async Task<CheckSetResponseDto> CheckSet(int gameId, CheckSetDto checkSetDto)
    {
        var game = await _gamesRepository.GetGameWithStatesAsync(gameId);

        if (game == null)
            return null;

        var cardList = checkSetDto.ToList();

        var cardStates = game.GameCardStates
            .Where(gs => cardList.Contains(gs.CardId) && gs.Location == CardLocation.Table)
            .Select(gs => gs.Card)
            .ToList();

        if (cardStates.Count != 3)
            return new CheckSetResponseDto { IsSet = false };

        var cards = await _cardsRepository.GetCardsByIds(cardStates.Select(c => c.Id).ToList());

        if (cards.Count != 3)
            return new CheckSetResponseDto { IsSet = false };

        bool isSet = _setValidationService.IsValidSet(cards[0], cards[1], cards[2]);
        bool existsFoundSet = await _foundSetsRepository.ExistsSet(gameId, cards[0].Id, cards[1].Id, cards[2].Id);

        if (!isSet || existsFoundSet)
            return new CheckSetResponseDto { IsSet = false };

        // discard the 3 found cards
        foreach (var gs in game.GameCardStates.Where(gs => cardList.Contains(gs.CardId)))
        {
            gs.Location = CardLocation.Discarded;
            await _gameCardStatesRepository.UpdateAsync(gs);
        }

        await _gameCardStatesRepository.SaveChangesAsync();

        // save the found set
        var foundSet = new FoundSet
        {
            GameId = game.Id,
            Card1Id = cards[0].Id,
            Card2Id = cards[1].Id,
            Card3Id = cards[2].Id
        };
        await _foundSetsRepository.AddAsync(foundSet);
        await _foundSetsRepository.SaveChangesAsync();

        // draw 3 replacement cards, preferring a combo that creates a valid set
        var newGameCardStates = await DrawSmartReplacements(game, 3);

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
            NewGameCardStates = _mapper.Map<List<GetGameCardStateDto>>(newGameCardStates),
            FoundSet = _mapper.Map<GetFoundSetDto>(foundSet),
            PossibleSetsCount = possibleSetsCount,
            IsFinished = game.IsFinished
        };
    }

    // Draws `amount` cards from the deck, preferring a combination that
    // creates at least one valid set with the existing table cards.
    // Table never exceeds 12 cards.
    private async Task<List<GameCardState>> DrawSmartReplacements(Game game, int amount)
    {
        var existingTableCards = GetGameCardStatesByLocation(game, CardLocation.Table)
            .Select(gs => gs.Card)
            .ToList();

        var deckStates = GetGameCardStatesByLocation(game, CardLocation.Deck);

        if (!deckStates.Any())
            return new List<GameCardState>();

        // find the best `amount` deck cards that create a valid set with the current table
        var bestDraw = FindBestDraw(existingTableCards, deckStates, amount)
                       ?? deckStates.Take(amount).ToList(); // fallback: draw top cards

        foreach (var gs in bestDraw)
        {
            gs.Location = CardLocation.Table;
            await _gameCardStatesRepository.UpdateAsync(gs);
        }

        await _gameCardStatesRepository.SaveChangesAsync();

        return bestDraw;
    }

    // Tries all combinations of `amount` deck cards to find one that produces
    // a valid set with the existing table cards. Returns null if none found.
    private List<GameCardState>? FindBestDraw(List<Card> tableCards, List<GameCardState> deckStates, int amount)
    {
        foreach (var combo in GetCombinations(deckStates, amount))
        {
            var combined = tableCards.Concat(combo.Select(gs => gs.Card)).ToList();
            if (_setValidationService.FindAllSets(combined).Count > 0)
                return combo;
        }

        return null;
    }

    private IEnumerable<List<T>> GetCombinations<T>(List<T> list, int k)
    {
        if (k == 0)
        {
            yield return new List<T>();
            yield break;
        }

        for (int i = 0; i < list.Count; i++)
        {
            var rest = list.Skip(i + 1).ToList();
            foreach (var combo in GetCombinations(rest, k - 1))
            {
                yield return new List<T> { list[i] }.Concat(combo).ToList();
            }
        }
    }

    // Used only at game creation — checks if initial 12 cards have a valid set.
    // If not, reshuffles all cards and redeals 12. Never adds extra cards.
    private async Task EnsureValidTable(Game game)
    {
        while (true)
        {
            var tableCards = GetGameCardStatesByLocation(game, CardLocation.Table)
                .Select(gs => gs.Card)
                .ToList();

            if (_setValidationService.FindAllSets(tableCards).Count > 0)
                return;

            if (!GetGameCardStatesByLocation(game, CardLocation.Deck).Any())
                return;

            // put all table cards back into deck
            var allTableStates = GetGameCardStatesByLocation(game, CardLocation.Table);
            foreach (var gs in allTableStates)
            {
                gs.Location = CardLocation.Deck;
                await _gameCardStatesRepository.UpdateAsync(gs);
            }

            await _gameCardStatesRepository.SaveChangesAsync();

            // reshuffle entire deck
            var allDeckCards = GetGameCardStatesByLocation(game, CardLocation.Deck);
            var shuffled = allDeckCards.OrderBy(_ => Random.Shared.Next()).ToList();
            for (int i = 0; i < shuffled.Count; i++)
            {
                shuffled[i].Order = i;
                await _gameCardStatesRepository.UpdateAsync(shuffled[i]);
            }

            await _gameCardStatesRepository.SaveChangesAsync();

            // redeal 12 cards
            await DrawCards(game, 12);
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
        var query = game.GameCardStates
            .Where(gs => gs.Location == location)
            .OrderBy(gs => gs.Order);

        return amount.HasValue
            ? query.Take(amount.Value).ToList()
            : query.ToList();
    }
}