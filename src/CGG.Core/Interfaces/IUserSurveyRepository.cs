using CGG.Core.Entities;

namespace CGG.Core.Interfaces;

public interface IUserSurveyRepository : IRepository<UserSurvey>
{
    /// <summary>Returns all InProgress/Completed UserSurveys for a user + surveyType.</summary>
    Task<IReadOnlyList<UserSurvey>> GetByUserAndTypeAsync(
        Guid userId, string surveyType, CancellationToken ct = default);
}
