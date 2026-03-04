using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CGG.Core.Interfaces;
using CGG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CGG.Infrastructure.Services;

public class SurveyManagementService : ISurveyManagementService
{
    private readonly ISurveyRepository _surveyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly SurveySeeder _surveySeeder;
    private readonly PromptSeeder _promptSeeder;
    private readonly ILogger<SurveyManagementService> _logger;

    public SurveyManagementService(
        ISurveyRepository surveyRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context,
        SurveySeeder surveySeeder,
        PromptSeeder promptSeeder,
        ILogger<SurveyManagementService> logger)
    {
        _surveyRepository = surveyRepository;
        _unitOfWork = unitOfWork;
        _context = context;
        _surveySeeder = surveySeeder;
        _promptSeeder = promptSeeder;
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

            var existingPrompts = await _context.AiPromptTemplates.ToListAsync(cancellationToken);
            if (existingPrompts.Count > 0)
            {
                _context.AiPromptTemplates.RemoveRange(existingPrompts);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _surveySeeder.SeedAsync();
            await _promptSeeder.SeedAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reloading surveys");
            return false;
        }
    }
}
