using CGG.Application.DTOs.Auth;
using CGG.Application.Interfaces;
using CGG.Application.Specifications;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace CGG.Application.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRepository<Family> _familyRepository;
    private readonly IRepository<Member> _memberRepository;
    private readonly IRepository<MemberRole> _memberRoleRepository;
    private readonly IReadRepository<Role> _roleReadRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthService _authService;
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(
        IUserRepository userRepository,
        IRepository<Family> familyRepository,
        IRepository<Member> memberRepository,
        IRepository<MemberRole> memberRoleRepository,
        IReadRepository<Role> roleReadRepository,
        IPasswordHasher passwordHasher,
        IAuthService authService,
        ILogger<RegistrationService> logger)
    {
        _userRepository = userRepository;
        _familyRepository = familyRepository;
        _memberRepository = memberRepository;
        _memberRoleRepository = memberRoleRepository;
        _roleReadRepository = roleReadRepository;
        _passwordHasher = passwordHasher;
        _authService = authService;
        _logger = logger;
    }

    public async Task<RegisterResult> RegisterAsync(
        Features.Auth.Commands.Register.RegisterCommand request,
        CancellationToken cancellationToken = default)
    {
        if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
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
        user.PasswordHash = _passwordHasher.Hash(request.Password);

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
            await _familyRepository.AddAsync(family, cancellationToken);

            user.FamilyId = family.Id;
            await _userRepository.AddAsync(user, cancellationToken);

            var member = new Member
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FamilyId = family.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _memberRepository.AddAsync(member, cancellationToken);

            var parentRole = await _roleReadRepository.FirstOrDefaultAsync(
                new RoleByNameSpec("parent"), cancellationToken);

            if (parentRole != null)
            {
                await _memberRoleRepository.AddAsync(new MemberRole
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
            await _userRepository.AddAsync(user, cancellationToken);
        }

        _logger.LogInformation("User prepared for registration: {UserId} {Email} {Role}",
            user.Id, user.Email, user.Role);

        // Resolve contexts — safe to call before save since it reads from the user object properties
        var availableContexts = await _authService.GetAvailableContextsAsync(user, cancellationToken);
        var activeContext = availableContexts.FirstOrDefault(c => c.Type == ContextType.Family)
            ?? availableContexts.First(c => c.Type == ContextType.System);
        var token = _authService.GenerateJwtToken(user, activeContext);

        return new RegisterResult(user, token, activeContext, availableContexts);
    }
}
