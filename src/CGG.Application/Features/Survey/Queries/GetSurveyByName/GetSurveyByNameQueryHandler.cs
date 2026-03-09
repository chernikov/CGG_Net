using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;
using CGG.Application.Interfaces;
using MediatR;

namespace CGG.Application.Features.Survey.Queries.GetSurveyByName;

public class GetSurveyByNameQueryHandler : IRequestHandler<GetSurveyByNameQuery, SurveyDetailDto?>
{
    private readonly ISurveyService _surveyService;

    public GetSurveyByNameQueryHandler(ISurveyService surveyService)
    {
        _surveyService = surveyService;
    }

    public async Task<SurveyDetailDto?> Handle(GetSurveyByNameQuery request, CancellationToken cancellationToken)
    {
        return await _surveyService.GetSurveyByNameAsync(request.Name, cancellationToken);
    }
}
