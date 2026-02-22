using CGG.Core.Entities;

namespace CGG.Application.Specifications;

public class RoleByNameSpec : BaseSpecification<Role>
{
    public RoleByNameSpec(string name)
        : base(r => r.Name == name) { }
}
