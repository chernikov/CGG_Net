using CGG.Core.Entities;

namespace CGG.Application.Specifications;

public class ChildrenByFamilyIdSpec : BaseSpecification<User>
{
    public ChildrenByFamilyIdSpec(Guid familyId)
        : base(u => u.FamilyId == familyId && u.Role == UserRole.UserChild)
    {
        ApplyOrderBy(u => u.FirstName!);
    }
}
