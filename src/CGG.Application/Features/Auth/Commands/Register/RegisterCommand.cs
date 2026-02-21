using CGG.Application.DTOs.Auth;
using CGG.Core.Entities;
using MediatR;

namespace CGG.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<RegisterResponseDto>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string Surname { get; set; }
    public UserRole Role { get; set; }
}
