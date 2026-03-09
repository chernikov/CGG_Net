using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;
using CGG.Application.Features.Survey.Commands.AnalyzeSurveyStep;
using CGG.Application.Features.Survey.Commands.ReloadSurveys;
using CGG.Application.Features.Survey.Commands.SaveFeedback;
using CGG.Application.Features.Survey.Commands.SaveSurveyAnswer;
using CGG.Application.Features.Survey.Commands.StartSurvey;
using CGG.Application.Features.Survey.Queries.GetAllSurveys;
using CGG.Application.Features.Survey.Queries.GetSurveyById;
using CGG.Application.Features.Survey.Queries.GetSurveyByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CGG.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurveyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SurveyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSurveys()
        {
            var surveys = await _mediator.Send(new GetAllSurveysQuery());
            return Ok(surveys);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurveyById(Guid id)
        {
            var survey = await _mediator.Send(new GetSurveyByIdQuery(id));
            if (survey == null) return NotFound();
            return Ok(survey);
        }

        [HttpGet("item")]
        public async Task<IActionResult> GetSurveyByName([FromQuery] string name)
        {
            var survey = await _mediator.Send(new GetSurveyByNameQuery(name));
            if (survey == null) return NotFound();
            return Ok(survey);
        }

        [HttpPost("reload")]
        public async Task<IActionResult> ReloadSurveys()
        {
            var result = await _mediator.Send(new ReloadSurveysCommand());
            if (!result)
                return StatusCode(500, new { Success = false, Error = "Failed to reload surveys. Check logs for details." });
            return Ok(new { Success = true, Message = "Surveys successfully reloaded." });
        }

        /// <summary>
        /// Start a new survey pass for the authenticated user.
        /// All previous InProgress/Completed surveys of the same type are marked Outdated.
        /// Returns the new UserSurveyId to be used in subsequent save-answer calls.
        /// </summary>
        [Authorize]
        [HttpPost("start")]
        public async Task<IActionResult> StartSurvey([FromBody] StartSurveyDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.SurveyType))
                return BadRequest(new { Error = "SurveyType is required." });

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _mediator.Send(new StartSurveyCommand(dto, userId.Value));
            if (!result.Success) return StatusCode(500, result);

            return Ok(result);
        }

        /// <summary>
        /// Persist answers for a survey step (idempotent — safe to call on every "Наступне" click).
        /// </summary>
        [Authorize]
        [HttpPost("save-answer")]
        public async Task<IActionResult> SaveAnswer([FromBody] SaveSurveyAnswerDto dto)
        {
            if (dto == null || dto.UserSurveyId == Guid.Empty || dto.StepNumber < 1)
                return BadRequest(new { Error = "UserSurveyId and StepNumber are required." });

            var result = await _mediator.Send(new SaveSurveyAnswerCommand(dto));
            if (!result.Success) return StatusCode(500, result);

            return Ok(result);
        }

        /// <summary>
        /// Analyse a completed survey step using AI.
        /// </summary>
        [HttpPost("analyze-step")]
        public async Task<IActionResult> AnalyzeStep([FromBody] SubmitSurveyStepDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.SurveyType) || dto.StepNumber < 1)
                return BadRequest(new { Error = "Invalid payload. SurveyType and StepNumber are required." });

            var result = await _mediator.Send(new AnalyzeSurveyStepCommand(dto));
            if (!result.Success) return StatusCode(500, result);

            return Ok(result);
        }

        /// <summary>
        /// Save user feedback (rating + comment) for a completed survey.
        /// </summary>
        [Authorize]
        [HttpPost("feedback")]
        public async Task<IActionResult> SaveFeedback([FromBody] SaveFeedbackDto dto)
        {
            if (dto == null || dto.UserSurveyId == Guid.Empty)
                return BadRequest(new { Error = "UserSurveyId is required." });

            var result = await _mediator.Send(new SaveFeedbackCommand(dto));
            if (!result.Success) return StatusCode(500, result);

            return Ok(result);
        }

        // ────────────────────────────────────────────────────────────────────
        private Guid? GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");
            return Guid.TryParse(claim, out var id) ? id : null;
        }
    }
}
