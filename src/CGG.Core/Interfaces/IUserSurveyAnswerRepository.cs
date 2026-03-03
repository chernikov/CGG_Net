using CGG.Core.Entities;

namespace CGG.Core.Interfaces;

public interface IUserSurveyAnswerRepository : IRepository<UserSurveyAnswer>
{
    /// <summary>Finds an existing answer by the unique key (UserSurveyId, StepNumber, QuestionId).</summary>
    Task<UserSurveyAnswer?> FindAsync(
        Guid userSurveyId, int stepNumber, string questionId, CancellationToken ct = default);
}
