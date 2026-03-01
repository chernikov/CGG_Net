using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CGG.Core.Interfaces;
using CGG.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace CGG.Infrastructure.Services;

public class SurveyManagementService : ISurveyManagementService
{
    private readonly ISurveyRepository _surveyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SurveySeeder _surveySeeder;
    private readonly ILogger<SurveyManagementService> _logger;

    public SurveyManagementService(
        ISurveyRepository surveyRepository,
        IUnitOfWork unitOfWork,
        SurveySeeder surveySeeder, 
        ILogger<SurveyManagementService> logger)
    {
        _surveyRepository = surveyRepository;
        _unitOfWork = unitOfWork;
        _surveySeeder = surveySeeder;
        _logger = logger;
    }

    public async Task<bool> ReloadSurveysAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var existingSurveys = await _surveyRepository.ListAllAsync(cancellationToken);
            if (existingSurveys.Any())
            {
                _surveyRepository.RemoveRange(existingSurveys);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _surveySeeder.SeedAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reloading surveys");
            return false;
        }
    }
}
