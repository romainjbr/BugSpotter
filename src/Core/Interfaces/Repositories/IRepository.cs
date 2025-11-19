
namespace Core.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken token);
    Task<List<T>> ListAsync(CancellationToken token);
    Task AddAsync(T entity, CancellationToken token);
    Task UpdateAsync(T entity, CancellationToken token);
    Task DeleteAsync(T entity, CancellationToken token);
}