using CGG.Application.DTOs.Auth;
using CGG.Application.Interfaces;
using CGG.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CGG.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        UserManager<User> userManager,
        IEmailService emailService,
        ILogger<RegisterCommandHandler> logger)
    {
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        // Create new user
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            DisplayName = request.DisplayName,
            Role = request.Role,
            EmailConfirmed = false,
            Credits = 0,
            CreatedAt = DateTime.UtcNow
        };

        // Create user with password
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to create user {Email}: {Errors}", request.Email, errors);
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        _logger.LogInformation("User created successfully: {UserId}, {Email}", user.Id, user.Email);

        // Send welcome email (don't fail registration if email fails)
        try
        {
            await _emailService.SendWelcomeEmailAsync(user.Email!, user.DisplayName, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send welcome email to {Email}", user.Email);
        }

        return new RegisterResponseDto
        {
            Message = "User registered successfully",
            UserId = user.Id
        };
    }
}
