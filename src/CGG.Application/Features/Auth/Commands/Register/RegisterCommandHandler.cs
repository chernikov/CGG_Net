using CGG.Application.DTOs.Auth;
using CGG.Application.Interfaces;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CGG.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher<User> passwordHasher,
        IEmailService emailService,
        ILogger<RegisterCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            DisplayName = request.DisplayName,
            Role = request.Role,
            EmailConfirmed = false,
            Credits = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PasswordHash = string.Empty // Temporary, will be set below
        };

        // Hash password
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        // Add user to database
        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User created successfully: {UserId}, {Email}", user.Id, user.Email);

        // Send welcome email (don't fail registration if email fails)
        try
        {
            await _emailService.SendWelcomeEmailAsync(user.Email, user.DisplayName, cancellationToken);
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
