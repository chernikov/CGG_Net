using CGG.Core.Entities;
using CGG.Core.Interfaces;
using CGG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CGG.Infrastructure.Repositories;

public class UserSurveyAnswerRepository : Repository<UserSurveyAnswer>, IUserSurveyAnswerRepository
{
    public UserSurveyAnswerRepository(ApplicationDbContext context) : base(context) { }

    public Task<UserSurveyAnswer?> FindAsync(
        Guid userSurveyId, int stepNumber, string questionId, CancellationToken ct = default)
        => _dbSet.FirstOrDefaultAsync(
            a => a.UserSurveyId == userSurveyId
              && a.StepNumber   == stepNumber
              && a.QuestionId   == questionId,
            ct);
}
