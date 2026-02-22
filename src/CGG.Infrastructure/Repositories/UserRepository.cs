using CGG.Core.Entities;
using CGG.Core.Interfaces;
using CGG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CGG.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<User?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbSet
            .Include(u => u.Family)
            .Include(u => u.Member)
            .Include(u => u.School)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
}
