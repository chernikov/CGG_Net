using System.Text.Json;
using CGG.Core.Entities;
using CGG.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CGG.Infrastructure.Data;

public class SurveyExampleSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SurveyExampleSeeder> _logger;

    public SurveyExampleSeeder(ApplicationDbContext context, ILogger<SurveyExampleSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        var basePath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "survey-examples");

        if (!Directory.Exists(basePath))
        {
            basePath = Path.Combine(Directory.GetCurrentDirectory(), "src", "CGG.Infrastructure", "Data", "SeedData", "survey-examples");
        }

        if (!Directory.Exists(basePath))
        {
            _logger.LogWarning("Survey example seed data directory not found at {BasePath}", basePath);
            return;
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        bool anyAdded = false;

        foreach (var surveyTypeDir in Directory.GetDirectories(basePath))
        {
            var surveyType = Path.GetFileName(surveyTypeDir);

            foreach (var filePath in Directory.GetFiles(surveyTypeDir, "*.json").OrderBy(f => f))
            {
                var json = await File.ReadAllTextAsync(filePath);
                var model = JsonSerializer.Deserialize<SurveyExampleProfileSeedModel>(json, options);
                if (model is null) continue;

                var existing = await _context.SurveyExampleProfiles
                    .Include(p => p.Answers)
                    .FirstOrDefaultAsync(p => p.SurveyType == surveyType && p.Slug == model.Slug);

                // Already seeded with answers — skip
                if (existing is not null && existing.Answers.Count > 0) continue;

                // Profile exists but has no answers (legacy row) — delete and re-create
                if (existing is not null)
                    _context.SurveyExampleProfiles.Remove(existing);

                var profile = new SurveyExampleProfile
                {
                    Id = Guid.NewGuid(),
                    SurveyType = surveyType,
                    Slug = model.Slug,
                    Icon = model.Icon,
                    NameUk = model.NameUk,
                    NameEn = model.NameEn,
                    SortOrder = model.SortOrder,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                foreach (var prop in model.Answers.EnumerateObject())
                {
                    profile.Answers.Add(new SurveyExampleAnswer
                    {
                        Id = Guid.NewGuid(),
                        Purpose = prop.Name,
                        ValueJson = prop.Value.GetRawText()
                    });
                }

                _context.SurveyExampleProfiles.Add(profile);
                anyAdded = true;
            }
        }

        if (anyAdded)
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Survey example profiles seeded successfully.");
        }
    }
}
