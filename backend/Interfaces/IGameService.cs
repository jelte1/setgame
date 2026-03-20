using backend.Entities;

namespace backend.Interfaces;

public interface IGameService
{
    Task<Game> CreateGameAsync(string userId);
}