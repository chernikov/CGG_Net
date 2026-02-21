using CGG.Application.DTOs.Auth;
using CGG.Application.Features.Auth.Commands.Register;
using CGG.Core.Entities;

namespace CGG.Application.Interfaces;

public record RegisterResult(
    User User,
    string Token,
    UserTokenContext ActiveContext,
    List<UserTokenContext> AvailableContexts);

public interface IRegistrationService
{
    Task<RegisterResult> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken = default);
}
