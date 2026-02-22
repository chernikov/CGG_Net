using CGG.Application.DTOs.Auth;
using MediatR;

namespace CGG.Application.Features.Auth.Commands.SwitchContext
{
    public class SwitchContextCommand : IRequest<SwitchContextResponseDto>
    {
        public required Guid UserId { get; set; }
        public required ContextType ContextType { get; set; }
        public Guid? ContextId { get; set; }
    }
}
