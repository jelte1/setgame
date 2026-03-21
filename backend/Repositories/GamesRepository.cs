using backend.Interfaces;
using backend.Database;
using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class GamesRepository : GenericRepository<Game>, IGamesRepository
{
    private readonly SetGameDbContext _context;

    public GamesRepository(SetGameDbContext context) : base(context)
    {
        this._context = context;
    }
    
    public async Task<Game?> GetGameWithUserAsync(int id)
    {
        return await _context.Games
            .Include(g => g.User)
            .FirstOrDefaultAsync(g => g.Id == id);
    }
    
    public async Task<Game?> GetGameWithStatesAsync(int id)
    {
        return await _context.Games
            .Include(g => g.GameStates)
            .FirstOrDefaultAsync(g => g.Id == id);
    }
    
     public async Task<Game?> GetGameWithStatesAndFoundSetsAsync(int id)
    {
        return await _context.Games
            .Include(g => g.GameStates)
            .Include(g => g.FoundSets)
            .FirstOrDefaultAsync(g => g.Id == id);
    }
}