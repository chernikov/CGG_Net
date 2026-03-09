using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CGG.Application.Features.Credits.Queries.GetCreditsBalance;
using CGG.Application.Features.Credits.Queries.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CGG.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CreditsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreditsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();

            var balance = await _mediator.Send(new GetCreditsBalanceQuery(userId.Value));
            return Ok(balance);
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();

            var transactions = await _mediator.Send(new GetTransactionsQuery(userId.Value, page, pageSize));
            return Ok(transactions);
        }

        private Guid? GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");
            return Guid.TryParse(claim, out var id) ? id : null;
        }
    }
}
