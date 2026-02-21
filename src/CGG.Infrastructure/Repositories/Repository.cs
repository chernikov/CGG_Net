using CGG.Core.Interfaces;
using CGG.Infrastructure.Data;

namespace CGG.Infrastructure.Repositories;

public class Repository<T> : ReadRepository<T>, IRepository<T> where T : class
{
    public Repository(ApplicationDbContext context) : base(context) { }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => await _dbSet.AddRangeAsync(entities, cancellationToken);

    public virtual void Update(T entity) => _dbSet.Update(entity);
    public virtual void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);
    public virtual void Remove(T entity) => _dbSet.Remove(entity);
    public virtual void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
}
