using backend.Entities;

namespace backend.Interfaces;

public interface IGameStatesRepository : IGenericRepository<GameState>
{
    Task<IEnumerable<GameState>> AddRangeAsync(IEnumerable<GameState> gameStates);
}