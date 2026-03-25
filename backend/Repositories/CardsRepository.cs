using backend.Interfaces;
using backend.Database;
using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class CardsRepository : GenericRepository<Card>, ICardsRepository
{
    private readonly SetGameDbContext _context;
    
    public CardsRepository(SetGameDbContext context) : base(context)
    {
        this._context = context;
    }
    
    public async Task<List<Card>> GetCardsByIds(List<int> ids)
    {
        return await _context.Cards.Where(c => ids.Contains(c.Id)).ToListAsync();
    }
}