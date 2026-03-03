using System;
using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CGG.Application.Features.Survey.Commands.StartSurvey;

public class StartSurveyCommandHandler
    : IRequestHandler<StartSurveyCommand, StartSurveyResponseDto>
{
    private readonly IUserSurveyRepository _userSurveyRepo;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<StartSurveyCommandHandler> _logger;

    public StartSurveyCommandHandler(
        IUserSurveyRepository userSurveyRepo,
        IUnitOfWork uow,
        ILogger<StartSurveyCommandHandler> logger)
    {
        _userSurveyRepo = userSurveyRepo;
        _uow = uow;
        _logger = logger;
    }

    public async Task<StartSurveyResponseDto> Handle(
        StartSurveyCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Payload;

        try
        {
            // ── 1. Mark all previous surveys of this type for this user as Outdated ──
            var existing = await _userSurveyRepo.GetByUserAndTypeAsync(
                request.UserId, dto.SurveyType, cancellationToken);

            foreach (var s in existing)
            {
                if (s.Status != UserSurveyStatus.Outdated)
                {
                    s.Status = UserSurveyStatus.Outdated;
                    _userSurveyRepo.Update(s);
                }
            }

            // ── 2. Create a fresh UserSurvey ─────────────────────────────────
            var survey = new UserSurvey
            {
                Id        = Guid.NewGuid(),
                UserId    = request.UserId,
                SurveyId  = dto.SurveyId == Guid.Empty ? null : dto.SurveyId,
                SurveyType = dto.SurveyType,
                Language  = dto.Language,
                Status    = UserSurveyStatus.InProgress,
                StartedAt = DateTime.UtcNow,
            };

            await _userSurveyRepo.AddAsync(survey, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Started survey {SurveyType} for user {UserId} → UserSurveyId={Id}",
                dto.SurveyType, request.UserId, survey.Id);

            return new StartSurveyResponseDto { UserSurveyId = survey.Id, Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start survey {SurveyType} for user {UserId}",
                dto.SurveyType, request.UserId);
            return new StartSurveyResponseDto { Success = false, Error = ex.Message };
        }
    }
}
