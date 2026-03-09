using System.Threading;
using System.Threading.Tasks;
using CGG.Core.Interfaces;
using MediatR;

namespace CGG.Application.Features.Survey.Commands.ReloadSurveys;

public class ReloadSurveysCommandHandler : IRequestHandler<ReloadSurveysCommand, bool>
{
    private readonly ISurveyManagementService _surveyManagementService;

    public ReloadSurveysCommandHandler(ISurveyManagementService surveyManagementService)
    {
        _surveyManagementService = surveyManagementService;
    }

    public async Task<bool> Handle(ReloadSurveysCommand request, CancellationToken cancellationToken)
    {
        return await _surveyManagementService.ReloadSurveysAsync(cancellationToken);
    }
}
