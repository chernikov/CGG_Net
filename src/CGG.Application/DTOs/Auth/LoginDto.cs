using CGG.Core.Entities;

namespace CGG.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public required string Token { get; set; }
        public required UserDto User { get; set; }
        public required UserTokenContext ActiveContext { get; set; }
        public List<UserTokenContext> AvailableContexts { get; set; } = [];
        // true only when user has BOTH family and school (or other multiple) contexts
        public bool CanSwitchContext => AvailableContexts.Count(c => c.Type != ContextType.System) > 1;
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public string? DisplayName { get; set; }
        public UserRole Role { get; set; }
        public decimal Credits { get; set; }
    }
}
