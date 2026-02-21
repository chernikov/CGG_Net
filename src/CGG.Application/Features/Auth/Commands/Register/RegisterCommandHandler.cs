using CGG.Application.DTOs.Auth;
using CGG.Application.Interfaces;
using CGG.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CGG.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly IRegistrationService _registrationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IRegistrationService registrationService,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<RegisterCommandHandler> logger)
    {
        _registrationService = registrationService;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _registrationService.RegisterAsync(request, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User registered: {UserId} {Email} {Role}",
            result.User.Id, result.User.Email, result.User.Role);

        try { await _emailService.SendWelcomeEmailAsync(result.User.Email, result.User.DisplayName, cancellationToken); }
        catch (Exception ex) { _logger.LogWarning(ex, "Welcome email failed for {Email}", result.User.Email); }

        return new RegisterResponseDto
        {
            Token = result.Token,
            User = new UserDto
            {
                Id = result.User.Id,
                Email = result.User.Email,
                DisplayName = result.User.DisplayName ?? string.Empty,
                Role = result.User.Role,
                Credits = result.User.Credits
            },
            ActiveContext = result.ActiveContext,
            AvailableContexts = result.AvailableContexts
        };
    }
}
