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
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher<User> passwordHasher,
        IAuthService authService,
        IEmailService emailService,
        ILogger<RegisterCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _authService = authService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists");

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
            PasswordHash = string.Empty
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        // -- Parent: create Family + Member + MemberRole(parent) --
        if (request.Role == UserRole.UserParent)
        {
            var family = new Family
            {
                Id = Guid.NewGuid(),
                Name = $"{request.DisplayName}\u2019s Family",
                Credits = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Repository<Family>().AddAsync(family, cancellationToken);

            user.FamilyId = family.Id;
            await _unitOfWork.Users.AddAsync(user, cancellationToken);

            var member = new Member
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FamilyId = family.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Repository<Member>().AddAsync(member, cancellationToken);

            // find the 'parent' role
            var parentRole = await _unitOfWork.Repository<Role>()
                .FirstOrDefaultAsync(r => r.Name == "parent", cancellationToken);

            if (parentRole != null)
            {
                await _unitOfWork.Repository<MemberRole>().AddAsync(new MemberRole
                {
                    MemberId = member.Id,
                    RoleId = parentRole.Id,
                    AssignedAt = DateTime.UtcNow
                }, cancellationToken);
            }

            user.MemberId = member.Id;
        }
        else
        {
            await _unitOfWork.Users.AddAsync(user, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User registered: {UserId} {Email} {Role}", user.Id, user.Email, user.Role);

        try { await _emailService.SendWelcomeEmailAsync(user.Email, user.DisplayName, cancellationToken); }
        catch (Exception ex) { _logger.LogWarning(ex, "Welcome email failed for {Email}", user.Email); }

        var availableContexts = await _authService.GetAvailableContextsAsync(user, cancellationToken);
        var activeContext = availableContexts.FirstOrDefault(c => c.Type == ContextType.Family)
            ?? availableContexts.First(c => c.Type == ContextType.System);
        var token = _authService.GenerateJwtToken(user, activeContext);

        return new RegisterResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName ?? string.Empty,
                Role = user.Role,
                Credits = user.Credits
            },
            ActiveContext = activeContext,
            AvailableContexts = availableContexts
        };
    }
}
