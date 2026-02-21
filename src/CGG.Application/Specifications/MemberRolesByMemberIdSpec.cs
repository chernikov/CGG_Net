using CGG.Core.Entities;

namespace CGG.Application.Specifications;

public class MemberRolesByMemberIdSpec : BaseSpecification<MemberRole>
{
    public MemberRolesByMemberIdSpec(Guid memberId)
        : base(mr => mr.MemberId == memberId) { }
}
