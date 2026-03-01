using System;
using System.Threading.Tasks;
using CGG.Application.Features.Survey.Commands.ReloadSurveys;
using CGG.Application.Features.Survey.Queries.GetAllSurveys;
using CGG.Application.Features.Survey.Queries.GetSurveyById;
using CGG.Application.Features.Survey.Queries.GetSurveyByName;
using MediatR;
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

            if (survey == null)
            {
                return NotFound();
            }

            return Ok(survey);
        }

        [HttpGet("item")]
        public async Task<IActionResult> GetSurveyByName([FromQuery] string name)
        {
            var survey = await _mediator.Send(new GetSurveyByNameQuery(name));

            if (survey == null)
            {
                return NotFound();
            }

            return Ok(survey);
        }

        [HttpPost("reload")]
        public async Task<IActionResult> ReloadSurveys()
        {
            var result = await _mediator.Send(new ReloadSurveysCommand());

            if (!result)
            {
                return StatusCode(500, new { Success = false, Error = "Failed to reload surveys. Check logs for details." });
            }

            return Ok(new { Success = true, Message = "Surveys successfully reloaded." });
        }
    }
}
