using CGG.Core.Entities;

namespace CGG.Application.DTOs.Auth;

public class RegisterRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string DisplayName { get; set; }
    public UserRole Role { get; set; } = UserRole.UserChild;
}

public class RegisterResponseDto
{
    public required string Message { get; set; }
    public Guid UserId { get; set; }
}
