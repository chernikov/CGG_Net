using MediatR;

namespace CGG.Application.Features.Family.Commands.AddChild;

public class AddChildCommand : IRequest<AddChildResponseDto>
{
    public Guid ParentUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
}

public class AddChildResponseDto
{
    public Guid ChildId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
}
