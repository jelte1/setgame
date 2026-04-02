using backend.Database;
using backend.Entities;
using backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class FoundSetsRepository : GenericRepository<FoundSet>, IFoundSetsRepository
{
    private readonly SetGameDbContext _context;
    
    public FoundSetsRepository(SetGameDbContext context) : base(context)
    {
        this._context = context;
    }

    public async Task<bool> ExistsSet(int gameId, int cardId1, int cardId2, int cardId3)
    {
        return await _context.Sets.Where(s => s.GameId == gameId && s.Card1Id == cardId1 && s.Card2Id == cardId2 && s.Card3Id == cardId3)
            .AnyAsync();;
    }
}