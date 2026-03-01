using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;
using CGG.Application.Interfaces;
using MediatR;

namespace CGG.Application.Features.Survey.Commands.AnalyzeSurveyStep;

public class AnalyzeSurveyStepCommandHandler
    : IRequestHandler<AnalyzeSurveyStepCommand, AiAnalysisResponseDto>
{
    private readonly IAiSurveyService _aiSurveyService;

    public AnalyzeSurveyStepCommandHandler(IAiSurveyService aiSurveyService)
    {
        _aiSurveyService = aiSurveyService;
    }

    public Task<AiAnalysisResponseDto> Handle(
        AnalyzeSurveyStepCommand request,
        CancellationToken cancellationToken)
        => _aiSurveyService.AnalyzeStepAsync(request.Payload, cancellationToken);
}
