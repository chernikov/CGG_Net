using System;
using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;
using CGG.Application.Interfaces;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CGG.Application.Features.Survey.Commands.AnalyzeSurveyStep;

public class AnalyzeSurveyStepCommandHandler
    : IRequestHandler<AnalyzeSurveyStepCommand, AiAnalysisResponseDto>
{
    private readonly IAiSurveyService _aiSurveyService;
    private readonly IRepository<AiLog> _aiLogRepo;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<AnalyzeSurveyStepCommandHandler> _logger;

    public AnalyzeSurveyStepCommandHandler(
        IAiSurveyService aiSurveyService,
        IRepository<AiLog> aiLogRepo,
        IUnitOfWork uow,
        ILogger<AnalyzeSurveyStepCommandHandler> logger)
    {
        _aiSurveyService = aiSurveyService;
        _aiLogRepo = aiLogRepo;
        _uow = uow;
        _logger = logger;
    }

    public async Task<AiAnalysisResponseDto> Handle(
        AnalyzeSurveyStepCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Payload;
        var result = await _aiSurveyService.AnalyzeStepAsync(dto, cancellationToken);

        // Persist AI log regardless of success/failure
        try
        {
            var log = new AiLog
            {
                Id            = Guid.NewGuid(),
                RequestType   = "survey_step",
                UserSurveyId  = dto.UserSurveyId,
                StepNumber    = dto.StepNumber,
                Response      = result.ResultJson,
                TokensUsed    = result.TokensUsed,
                Status        = result.Success ? "success" : "error",
                ErrorMessage  = result.Error,
                CreatedAt     = DateTime.UtcNow,
            };

            await _aiLogRepo.AddAsync(log, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Non-fatal: log but don't fail the request
            _logger.LogWarning(ex,
                "Failed to persist AiLog for survey step {Step} / UserSurvey {UserSurveyId}",
                dto.StepNumber, dto.UserSurveyId);
        }

        return result;
    }
}
