using backend.Database;
using backend.Entities;
using backend.Interfaces;

namespace backend.Repositories;

public class FoundSetsRepository : GenericRepository<FoundSet>, IFoundSetsRepository
{
    private readonly SetGameDbContext _context;
    
    public FoundSetsRepository(SetGameDbContext context) : base(context)
    {
        this._context = context;
    }
}