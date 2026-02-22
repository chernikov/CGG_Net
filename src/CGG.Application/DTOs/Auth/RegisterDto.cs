using CGG.Core.Entities;

namespace CGG.Application.DTOs.Auth;

public class RegisterRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string Surname { get; set; }
    public UserRole Role { get; set; } = UserRole.UserChild;
}

public class RegisterResponseDto
{
    public required string Token { get; set; }
    public required UserDto User { get; set; }
    public required UserTokenContext ActiveContext { get; set; }
    public List<UserTokenContext> AvailableContexts { get; set; } = [];
    public bool CanSwitchContext => AvailableContexts.Count(c => c.Type != ContextType.System) > 1;
}
