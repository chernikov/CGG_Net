using CGG.Application.Interfaces;
using CGG.Application.Specifications;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CGG.Application.Features.Family.Commands.AddChild;

public class AddChildCommandHandler : IRequestHandler<AddChildCommand, AddChildResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRepository<Member> _memberRepository;
    private readonly IRepository<MemberRole> _memberRoleRepository;
    private readonly IReadRepository<Role> _roleReadRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddChildCommandHandler> _logger;

    public AddChildCommandHandler(
        IUserRepository userRepository,
        IRepository<Member> memberRepository,
        IRepository<MemberRole> memberRoleRepository,
        IReadRepository<Role> roleReadRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        ILogger<AddChildCommandHandler> logger)
    {
        _userRepository = userRepository;
        _memberRepository = memberRepository;
        _memberRoleRepository = memberRoleRepository;
        _roleReadRepository = roleReadRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AddChildResponseDto> Handle(AddChildCommand request, CancellationToken cancellationToken)
    {
        var parent = await _userRepository.GetByIdAsync(request.ParentUserId, cancellationToken);
        if (parent == null)
            throw new InvalidOperationException("Parent user not found");

        if (!parent.FamilyId.HasValue)
            throw new InvalidOperationException("Parent does not belong to a family");

        var childEmail = string.IsNullOrWhiteSpace(request.Email) 
            ? $"child_{Guid.NewGuid()}@cgg.local" 
            : request.Email;

        if (await _userRepository.EmailExistsAsync(childEmail, cancellationToken))
            throw new InvalidOperationException("User with this email already exists");

        // Generate a random password for the child
        var randomPassword = Guid.NewGuid().ToString("N");

        var childUser = new User
        {
            Id = Guid.NewGuid(),
            Email = childEmail,
            FirstName = request.Name,
            Role = UserRole.UserChild,
            EmailConfirmed = false,
            Credits = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            FamilyId = parent.FamilyId,
            Age = request.Age,
            AgeAddedDate = DateTime.UtcNow,
            PasswordHash = _passwordHasher.Hash(randomPassword)
        };

        await _userRepository.AddAsync(childUser, cancellationToken);

        var member = new Member
        {
            Id = Guid.NewGuid(),
            UserId = childUser.Id,
            FamilyId = parent.FamilyId.Value,
            CreatedAt = DateTime.UtcNow
        };
        await _memberRepository.AddAsync(member, cancellationToken);

        var childRole = await _roleReadRepository.FirstOrDefaultAsync(
            new RoleByNameSpec("child"), cancellationToken);

        if (childRole != null)
        {
            await _memberRoleRepository.AddAsync(new MemberRole
            {
                MemberId = member.Id,
                RoleId = childRole.Id,
                AssignedAt = DateTime.UtcNow
            }, cancellationToken);
        }

        childUser.MemberId = member.Id;
        // No explicit Update() needed — EF tracks the entity from AddAsync and picks up the MemberId change automatically

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Child added: {ChildId} to Family: {FamilyId}", childUser.Id, parent.FamilyId);

        return new AddChildResponseDto
        {
            ChildId = childUser.Id,
            Name = childUser.FirstName ?? string.Empty,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : childUser.Email,
            Age = request.Age,
            Gender = request.Gender
        };
    }
}
