using System;
using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;
using CGG.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CGG.Application.Features.Survey.Commands.SaveFeedback;

public class SaveFeedbackCommandHandler
    : IRequestHandler<SaveFeedbackCommand, SaveFeedbackResponseDto>
{
    private readonly IUserSurveyRepository _surveyRepo;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<SaveFeedbackCommandHandler> _logger;

    public SaveFeedbackCommandHandler(
        IUserSurveyRepository surveyRepo,
        IUnitOfWork uow,
        ILogger<SaveFeedbackCommandHandler> logger)
    {
        _surveyRepo = surveyRepo;
        _uow = uow;
        _logger = logger;
    }

    public async Task<SaveFeedbackResponseDto> Handle(
        SaveFeedbackCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Payload;

        try
        {
            var survey = await _surveyRepo.GetByIdAsync(dto.UserSurveyId, cancellationToken)
                ?? throw new InvalidOperationException($"UserSurvey {dto.UserSurveyId} not found");

            survey.FeedbackRating  = dto.Rating;
            survey.FeedbackComment = dto.Comment?.Trim();
            survey.CompletedAt     = DateTime.UtcNow;

            _surveyRepo.Update(survey);
            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Saved feedback for UserSurvey {UserSurveyId}: rating={Rating}",
                dto.UserSurveyId, dto.Rating);

            return new SaveFeedbackResponseDto { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save feedback for UserSurvey {UserSurveyId}", dto.UserSurveyId);
            return new SaveFeedbackResponseDto { Success = false, Error = ex.Message };
        }
    }
}
