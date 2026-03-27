using backend.Interfaces;
using backend.Database;
using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class UsersRepository : GenericRepository<User>, IUsersRepository
{
    private readonly SetGameDbContext _context;
    
    public UsersRepository(SetGameDbContext context) : base(context)
    {
        this._context = context;
    }

    public async Task<List<Game>?> GetUserGames(string userId)
    {
        return await _context.Games
            .Include(g => g.GameStates)
            .ThenInclude(gs => gs.Card)
            .Include(g => g.FoundSets)
            .Where(g => g.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<User?> GetUserByUserId(string userId)
    {
        // no nullcheck necessary, method returns null if no records
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}