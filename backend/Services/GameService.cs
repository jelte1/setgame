using backend.Entities;
using backend.Interfaces;

namespace backend.Services;

public class GameService : IGameService
{
    private readonly IGamesRepository _gamesRepository;
    private readonly ICardsRepository _cardsRepository;
    private readonly IGameStatesRepository _gameStateRepository;
    
    public GameService(IGamesRepository gamesRepository, IGameStatesRepository gameStateRepository, ICardsRepository cardsRepository)
    {
        _gamesRepository = gamesRepository;
        _cardsRepository = cardsRepository;
        _gameStateRepository = gameStateRepository;
    }
    
    public async Task<Game> CreateGameAsync(string userId)
    {
        var game = new Game
        {
            UserId = userId,
            CreatedAt = DateTime.Now
        };
        await _gamesRepository.AddAsync(game);
        
        var cards = await _cardsRepository.GetAllAsync();
        var shuffled = cards.OrderBy(_ => Guid.NewGuid()).ToList();
        
        for (int i = 0; i < shuffled.Count; i++)
        {
            var gameState = new GameState
            {
                GameId = game.Id,
                CardId = shuffled[i].Id,
                Location = CardLocation.Deck,
                Order = i
            };
            await _gameStateRepository.AddAsync(gameState);
        }
        
        var first12 = game.GameStates
            .OrderBy(gs => gs.Order)
            .Take(12)
            .ToList();

        foreach (var gs in first12)
        {
            gs.Location = CardLocation.Table;
            await _gameStateRepository.UpdateAsync(gs);
        }

        return game;
    }
}