using CGG.Core.Entities;

namespace CGG.Application.Specifications;

public class MemberByUserIdSpec : BaseSpecification<Member>
{
    public MemberByUserIdSpec(Guid userId)
        : base(m => m.UserId == userId)
    {
        AddInclude(m => m.MemberRoles);
    }
}
