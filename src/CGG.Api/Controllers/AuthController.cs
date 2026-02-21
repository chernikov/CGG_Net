using AutoMapper;
using CGG.Application.DTOs.Auth;
using CGG.Application.Features.Auth.Commands.Login;
using CGG.Application.Features.Auth.Commands.Register;
using CGG.Application.Features.Auth.Commands.SwitchContext;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CGG.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto, CancellationToken cancellationToken)
        {
            try
            {
                // Map DTO to Command
                var command = _mapper.Map<RegisterCommand>(requestDto);

                // Send command through MediatR
                var response = await _mediator.Send(command, cancellationToken);

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto, CancellationToken cancellationToken)
        {
            try
            {
                // Map DTO to Command
                var command = _mapper.Map<LoginCommand>(requestDto);

                // Send command through MediatR
                var response = await _mediator.Send(command, cancellationToken);

                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
            }
        }

        [HttpPost("switch-context")]
        [Authorize]
        public async Task<IActionResult> SwitchContext([FromBody] SwitchContextRequestDto requestDto, CancellationToken cancellationToken)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized(new { message = "Invalid token" });

                var command = new SwitchContextCommand
                {
                    UserId = userId,
                    ContextType = requestDto.ContextType,
                    ContextId = requestDto.ContextId
                };

                var response = await _mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during context switch", error = ex.Message });
            }
        }
    }
}
