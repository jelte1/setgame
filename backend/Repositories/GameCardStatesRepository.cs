using backend.Database;
using backend.Entities;
using backend.Interfaces;

namespace backend.Repositories;

public class GameCardStatesRepository : GenericRepository<GameCardState>, IGameCardStatesRepository
{
    private readonly SetGameDbContext _context;
    
    public GameCardStatesRepository(SetGameDbContext context) : base(context)
    {
        this._context = context;
    }
    
    public async Task<IEnumerable<GameCardState>> AddRangeAsync(IEnumerable<GameCardState> gameCardStates)
    {
        await _context.AddRangeAsync(gameCardStates);
        return gameCardStates;
    }
    
    public async Task UpdateRangeAsync(IEnumerable<GameCardState> entities)
    {
        _context.GameCardStates.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }
}