using CGG.Application.DTOs.Auth;
using CGG.Application.Interfaces;
using CGG.Core.Interfaces;
using MediatR;

namespace CGG.Application.Features.Auth.Commands.SwitchContext;

public class SwitchContextCommandHandler : IRequestHandler<SwitchContextCommand, SwitchContextResponseDto>
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public SwitchContextCommandHandler(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    public async Task<SwitchContextResponseDto> Handle(SwitchContextCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found");

        var availableContexts = await _authService.GetAvailableContextsAsync(user, cancellationToken);

        UserTokenContext? targetContext = request.ContextType == ContextType.System
            ? availableContexts.FirstOrDefault(c => c.Type == ContextType.System)
            : availableContexts.FirstOrDefault(c =>
                c.Type == request.ContextType &&
                c.ContextId == request.ContextId);

        if (targetContext == null)
            throw new UnauthorizedAccessException(
                $"User does not have access to context {request.ContextType} / {request.ContextId}");

        var token = _authService.GenerateJwtToken(user, targetContext);

        return new SwitchContextResponseDto
        {
            Token = token,
            ActiveContext = targetContext
        };
    }
}
