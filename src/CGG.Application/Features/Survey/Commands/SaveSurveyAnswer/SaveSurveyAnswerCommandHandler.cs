using System;
using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CGG.Application.Features.Survey.Commands.SaveSurveyAnswer;

public class SaveSurveyAnswerCommandHandler
    : IRequestHandler<SaveSurveyAnswerCommand, SaveSurveyAnswerResponseDto>
{
    private readonly IUserSurveyAnswerRepository _answerRepo;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<SaveSurveyAnswerCommandHandler> _logger;

    public SaveSurveyAnswerCommandHandler(
        IUserSurveyAnswerRepository answerRepo,
        IUnitOfWork uow,
        ILogger<SaveSurveyAnswerCommandHandler> logger)
    {
        _answerRepo = answerRepo;
        _uow = uow;
        _logger = logger;
    }

    public async Task<SaveSurveyAnswerResponseDto> Handle(
        SaveSurveyAnswerCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Payload;

        try
        {
            // Upsert each answer — idempotent on (UserSurveyId, StepNumber, QuestionId)
            foreach (var a in dto.Answers)
            {
                if (string.IsNullOrWhiteSpace(a.Answer)) continue;

                var existing = await _answerRepo.FindAsync(
                    dto.UserSurveyId, dto.StepNumber, a.QuestionId, cancellationToken);

                if (existing is null)
                {
                    await _answerRepo.AddAsync(new UserSurveyAnswer
                    {
                        Id            = Guid.NewGuid(),
                        UserSurveyId  = dto.UserSurveyId,
                        StepNumber    = dto.StepNumber,
                        QuestionId    = a.QuestionId,
                        QuestionText  = a.QuestionText,
                        Answer        = a.Answer,
                        CreatedAt     = DateTime.UtcNow,
                        UpdatedAt     = DateTime.UtcNow,
                    }, cancellationToken);
                }
                else
                {
                    existing.Answer       = a.Answer;
                    existing.QuestionText = a.QuestionText;
                    existing.UpdatedAt    = DateTime.UtcNow;
                    _answerRepo.Update(existing);
                }
            }

            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Saved {Count} answers for UserSurvey {UserSurveyId} step {Step}",
                dto.Answers.Count, dto.UserSurveyId, dto.StepNumber);

            return new SaveSurveyAnswerResponseDto { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to save answers for UserSurvey {UserSurveyId} step {Step}",
                dto.UserSurveyId, dto.StepNumber);
            return new SaveSurveyAnswerResponseDto { Success = false, Error = ex.Message };
        }
    }
}
