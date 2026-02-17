using CGG.Core.Entities;

namespace CGG.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, User? User, string? ErrorMessage)> ValidateUserCredentialsAsync(
            string email, 
            string password, 
            CancellationToken cancellationToken = default);
        
        string GenerateJwtToken(User user);
    }
}
