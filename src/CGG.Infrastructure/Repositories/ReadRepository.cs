using CGG.Core.Interfaces;
using CGG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CGG.Infrastructure.Repositories;

public class ReadRepository<T> : IReadRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public ReadRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync([id], cancellationToken);

    public virtual async Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec)
            .FirstOrDefaultAsync(cancellationToken);

    public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec)
            .ToListAsync(cancellationToken);

    public virtual async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken);

    public virtual async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec)
            .CountAsync(cancellationToken);

    public virtual async Task<bool> AnyAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec)
            .AnyAsync(cancellationToken);
}
