using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CGG.Application.DTOs.Credits;
using CGG.Application.Features.Credits.Commands.CreateMonobankPayment;
using CGG.Application.Features.Credits.Commands.HandleMonobankWebhook;
using CGG.Application.Features.Credits.Queries.GetCreditsBalance;
using CGG.Application.Features.Credits.Queries.GetTransactions;
using CGG.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CGG.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CreditsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreditsController> _logger;

        public CreditsController(IMediator mediator, ILogger<CreditsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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

        /// <summary>
        /// Create a Monobank invoice to top up credits.
        /// Logic: &lt; 300 UAH → 1:1, ≥ 300 UAH → +40% bonus credits.
        /// </summary>
        [HttpPost("payment/create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequestDto dto)
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();

            if (dto.AmountUAH < 1m)
                return BadRequest(new { error = "Мінімальна сума — 1 грн." });

            try
            {
                var result = await _mediator.Send(new CreateMonobankPaymentCommand(userId.Value, dto.AmountUAH));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Monobank payment for user {UserId}", userId);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Webhook endpoint called by Monobank when payment status changes.
        /// This endpoint is intentionally unauthenticated (called by Monobank server).
        /// </summary>
        [AllowAnonymous]
        [HttpPost("payment/webhook")]
        public async Task<IActionResult> MonobankWebhook([FromBody] MonobankWebhookPayload payload)
        {
            _logger.LogInformation(
                "Monobank webhook received: invoiceId={InvoiceId}, status={Status}",
                payload.InvoiceId, payload.Status);

            try
            {
                await _mediator.Send(new HandleMonobankWebhookCommand(payload));
                return Ok(new { ok = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Monobank webhook processing failed");
                // Return 200 to prevent Monobank from retrying on our own errors
                return Ok(new { ok = false, error = ex.Message });
            }
        }

        private Guid? GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");
            return Guid.TryParse(claim, out var id) ? id : null;
        }
    }
}
