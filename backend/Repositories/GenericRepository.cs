using backend.Interfaces;
using backend.Database;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;

namespace backend.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly SetGameDbContext _context;
    
    public GenericRepository(SetGameDbContext context)
    {
        this._context = context;
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.AddAsync(entity);
        return entity;
    }

    public async Task<T?> GetAsync(int? id)
    {
        if (id == null)
        {
            return null;
        }
        
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Update(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        if (entity == null)
        {
            return;
        }
        
        _context.Set<T>().Remove(entity);
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync(id);
        return entity != null;
    }
}