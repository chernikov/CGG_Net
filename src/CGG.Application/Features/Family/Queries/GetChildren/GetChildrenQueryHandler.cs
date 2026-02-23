using CGG.Application.Specifications;
using CGG.Core.Interfaces;
using MediatR;

namespace CGG.Application.Features.Family.Queries.GetChildren;

public class GetChildrenQueryHandler : IRequestHandler<GetChildrenQuery, List<ChildDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IReadRepository<Core.Entities.User> _userReadRepository;

    public GetChildrenQueryHandler(
        IUserRepository userRepository,
        IReadRepository<Core.Entities.User> userReadRepository)
    {
        _userRepository = userRepository;
        _userReadRepository = userReadRepository;
    }

    public async Task<List<ChildDto>> Handle(GetChildrenQuery request, CancellationToken cancellationToken)
    {
        var parent = await _userRepository.GetByIdAsync(request.ParentUserId, cancellationToken);
        if (parent == null || !parent.FamilyId.HasValue)
            return [];

        var children = await _userReadRepository.ListAsync(
            new ChildrenByFamilyIdSpec(parent.FamilyId.Value), cancellationToken);

        return children.Select(c => new ChildDto
        {
            Id = c.Id,
            Name = c.FirstName ?? string.Empty,
            Email = c.Email.EndsWith("@cgg.local") ? null : c.Email,
            Age = c.Age,
            Gender = string.Empty, // Gender is not stored in User entity yet
            CreatedAt = c.CreatedAt
        }).ToList();
    }
}
