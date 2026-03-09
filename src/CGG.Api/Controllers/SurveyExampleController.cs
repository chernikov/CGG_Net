using CGG.Application.Features.SurveyExample.Queries.GetSurveyExampleProfiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CGG.Api.Controllers;

[ApiController]
[Route("api/survey-example")]
public class SurveyExampleController : ControllerBase
{
    private readonly IMediator _mediator;

    public SurveyExampleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfiles([FromQuery] string name, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("name is required");

        var result = await _mediator.Send(new GetSurveyExampleProfilesQuery(name), ct);
        return Ok(result);
    }
}
