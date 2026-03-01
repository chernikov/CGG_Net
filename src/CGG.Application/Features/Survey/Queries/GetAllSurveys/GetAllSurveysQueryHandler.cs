using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;
using CGG.Application.Interfaces;
using MediatR;

namespace CGG.Application.Features.Survey.Queries.GetAllSurveys;

public class GetAllSurveysQueryHandler : IRequestHandler<GetAllSurveysQuery, List<SurveySummaryDto>>
{
    private readonly ISurveyService _surveyService;

    public GetAllSurveysQueryHandler(ISurveyService surveyService)
    {
        _surveyService = surveyService;
    }

    public async Task<List<SurveySummaryDto>> Handle(GetAllSurveysQuery request, CancellationToken cancellationToken)
    {
        return await _surveyService.GetAllSurveysAsync(cancellationToken);
    }
}
