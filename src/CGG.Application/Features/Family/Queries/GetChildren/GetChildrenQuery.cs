using MediatR;

namespace CGG.Application.Features.Family.Queries.GetChildren;

public class GetChildrenQuery : IRequest<List<ChildDto>>
{
    public Guid ParentUserId { get; set; }
}

public class ChildDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int? Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
