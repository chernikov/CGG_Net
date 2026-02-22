using CGG.Application.DTOs.Auth;
using MediatR;

namespace CGG.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
