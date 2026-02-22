using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CGG.Application.Features.Family.Commands.AddChild;

namespace CGG.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FamilyController : ControllerBase
{
    private readonly IMediator _mediator;

    public FamilyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("children")]
    public async Task<IActionResult> AddChild([FromBody] AddChildRequestDto requestDto, CancellationToken cancellationToken)
    {
        try
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Invalid token" });

            var command = new AddChildCommand
            {
                ParentUserId = userId,
                Name = requestDto.Name,
                Email = requestDto.Email,
                Age = requestDto.Age,
                Gender = requestDto.Gender
            };

            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while adding the child", error = ex.Message });
        }
    }
}

public class AddChildRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
}
