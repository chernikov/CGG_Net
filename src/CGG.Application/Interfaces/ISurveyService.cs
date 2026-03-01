using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;

namespace CGG.Application.Interfaces;

public interface ISurveyService
{
    Task<List<SurveySummaryDto>> GetAllSurveysAsync(CancellationToken cancellationToken = default);
    Task<SurveyDetailDto?> GetSurveyByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SurveyDetailDto?> GetSurveyByNameAsync(string name, CancellationToken cancellationToken = default);
}
