using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly BugDb _db;
    
    public EfRepository(BugDb db)
    { 
        _db = db;
    }

    public async Task AddAsync(T entity, CancellationToken token)
    {
        _db.Set<T>().Add(entity);
        await _db.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(T entity, CancellationToken token)
    {
        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync(token);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var item = await _db.Set<T>().FindAsync(id);
        return item;
    }

    public async Task<List<T>> ListAsync(CancellationToken token)
    {
        var list = await _db.Set<T>().AsNoTracking().ToListAsync(token);

        return list;
    }

    public async Task UpdateAsync(T entity, CancellationToken token)
    {
        _db.Set<T>().Update(entity); 
        await _db.SaveChangesAsync(token);
    }
}
