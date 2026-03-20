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
}