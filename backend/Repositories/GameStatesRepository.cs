using backend.Database;
using backend.Entities;
using backend.Interfaces;

namespace backend.Repositories;

public class GameStatesRepository : GenericRepository<GameState>, IGameStatesRepository
{
    private readonly SetGameDbContext _context;
    
    public GameStatesRepository(SetGameDbContext context) : base(context)
    {
        this._context = context;
    }
    
    public async Task<IEnumerable<GameState>> AddRangeAsync(IEnumerable<GameState> gameStates)
    {
        await _context.AddRangeAsync(gameStates);
        return gameStates;
    }
}