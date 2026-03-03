using CGG.Core.Entities;
using CGG.Core.Interfaces;
using CGG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CGG.Infrastructure.Repositories;

public class UserSurveyRepository : Repository<UserSurvey>, IUserSurveyRepository
{
    public UserSurveyRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<UserSurvey>> GetByUserAndTypeAsync(
        Guid userId, string surveyType, CancellationToken ct = default)
        => await _dbSet
            .Where(s => s.UserId == userId && s.SurveyType == surveyType)
            .ToListAsync(ct);
}
