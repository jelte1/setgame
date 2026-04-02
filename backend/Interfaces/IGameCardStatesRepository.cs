using backend.Entities;

namespace backend.Interfaces;

public interface IGameCardStatesRepository : IGenericRepository<GameCardState>
{
    Task<IEnumerable<GameCardState>> AddRangeAsync(IEnumerable<GameCardState> gameCardStates);
    Task UpdateRangeAsync(IEnumerable<GameCardState> entities);
}