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
                Id               = Guid.NewGuid(),
                RequestType      = "survey_step",
                UserId           = request.UserId,
                UserSurveyId     = dto.UserSurveyId,
                PromptTemplateId = result.PromptTemplateId,
                StepNumber       = dto.StepNumber,
                Provider         = result.Provider,
                Model            = result.Model,
                SystemPrompt     = result.SystemPrompt,
                PromptText       = result.PromptText,
                UserInput        = result.UserInput,
                Response         = result.ResultJson,
                TokensUsed       = result.TokensUsed,
                PromptTokens     = result.PromptTokens,
                CompletionTokens = result.CompletionTokens,
                ProcessingTimeMs = result.ProcessingTimeMs,
                Status           = result.Success ? "success" : "error",
                ErrorMessage     = result.Error,
                ErrorCode        = result.ErrorCode,
                CostUsd          = ComputeCostUsd(result.Model, result.PromptTokens, result.CompletionTokens),
                CreatedAt        = DateTime.UtcNow,
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

    /// <summary>Estimates cost in USD based on model pricing.</summary>
    private static decimal? ComputeCostUsd(string? model, int? promptTokens, int? completionTokens)
    {
        if (model is null || promptTokens is null || completionTokens is null)
            return null;

        // Prices per 1M tokens (input / output)
        var (inputPer1M, outputPer1M) = model switch
        {
            var m when m.Contains("nano") => (0.05m, 0.40m),
            var m when m.Contains("mini") => (0.25m, 2.00m),
            _                            => (0.25m, 2.00m)
        };

        return (promptTokens.Value * inputPer1M + completionTokens.Value * outputPer1M)
               / 1_000_000m;
    }}