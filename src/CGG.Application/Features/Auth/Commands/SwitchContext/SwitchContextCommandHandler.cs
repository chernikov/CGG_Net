using CGG.Application.DTOs.Auth;
using CGG.Application.Interfaces;
using CGG.Core.Interfaces;
using MediatR;

namespace CGG.Application.Features.Auth.Commands.SwitchContext
{
    public class SwitchContextCommandHandler : IRequestHandler<SwitchContextCommand, SwitchContextResponseDto>
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public SwitchContextCommandHandler(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<SwitchContextResponseDto> Handle(SwitchContextCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new UnauthorizedAccessException("User not found");

            var availableContexts = await _authService.GetAvailableContextsAsync(user, cancellationToken);

            // Find the requested context
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
}
