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
