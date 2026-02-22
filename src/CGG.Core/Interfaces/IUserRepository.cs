using CGG.Core.Entities;

namespace CGG.Core.Interfaces;

/// <summary>
/// Domain-specific user repository. Extends full CRUD (IRepository) with user-specific read methods.
/// Inject IUserRepository when you need both reads and writes on User,
/// or IReadRepository&lt;User&gt; when you only read.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
