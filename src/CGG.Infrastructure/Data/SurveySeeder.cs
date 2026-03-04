using System.Text.Json;
using CGG.Core.Entities;
using CGG.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CGG.Infrastructure.Data
{
    public class SurveySeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SurveySeeder> _logger;

        public SurveySeeder(ApplicationDbContext context, ILogger<SurveySeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await SeedLanguagesAsync();
            await SeedSurveysAsync();
        }

        private async Task SeedLanguagesAsync()
        {
            var languages = new[]
            {
                new Language { Id = Guid.NewGuid(), Code = "en", Name = "English", NativeName = "English", IsDefault = true, SortOrder = 1 },
                new Language { Id = Guid.NewGuid(), Code = "uk", Name = "Ukrainian", NativeName = "Українська", IsDefault = false, SortOrder = 2 },
                new Language { Id = Guid.NewGuid(), Code = "hi", Name = "Hindi", NativeName = "हिन्दी", IsDefault = false, SortOrder = 3 }
            };

            foreach (var lang in languages)
            {
                if (!await _context.Languages.AnyAsync(l => l.Code == lang.Code))
                {
                    _context.Languages.Add(lang);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedSurveysAsync()
        {
            var defaultLanguage = await _context.Languages.FirstOrDefaultAsync(l => l.IsDefault) 
                ?? await _context.Languages.FirstAsync();
            
            var languages = await _context.Languages.ToDictionaryAsync(l => l.Code, l => l.Id);

            var surveyTypes = new[] { "ab-test", "intro", "parent-child-talents", "parent" };
            var basePath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "surveys");

            // If running from source, adjust path
            if (!Directory.Exists(basePath))
            {
                basePath = Path.Combine(Directory.GetCurrentDirectory(), "src", "CGG.Infrastructure", "Data", "SeedData", "surveys");
            }

            if (!Directory.Exists(basePath))
            {
                _logger.LogWarning($"Survey seed data directory not found at {basePath}");
                return;
            }

            foreach (var surveyType in surveyTypes)
            {
                if (await _context.Surveys.AnyAsync(s => s.SurveyType == surveyType))
                {
                    continue; // Already seeded
                }

                var surveyDir = Path.Combine(basePath, surveyType);
                if (!Directory.Exists(surveyDir)) continue;

                var survey = new Survey
                {
                    Id = Guid.NewGuid(),
                    SurveyType = surveyType,
                    Title = GetSurveyTitle(surveyType),
                    Description = GetSurveyDescription(surveyType),
                    DefaultLanguageId = defaultLanguage.Id,
                    IsActive = true,
                    IsPublic = true,
                    Version = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Surveys.Add(survey);

                var stepFiles = Directory.GetFiles(surveyDir, "step*.json").OrderBy(f => f).ToList();
                
                foreach (var stepFile in stepFiles)
                {
                    var json = await File.ReadAllTextAsync(stepFile);
                    var stepData = JsonSerializer.Deserialize<SurveyStepSeedModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (stepData == null) continue;

                    var surveyStep = new SurveyStep
                    {
                        Id = Guid.NewGuid(),
                        SurveyId = survey.Id,
                        StepNumber = stepData.Step,
                        IsRequired = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.SurveySteps.Add(surveyStep);

                    int questionSortOrder = 1;
                    foreach (var qData in stepData.Questions)
                    {
                        var question = new SurveyQuestion
                        {
                            Id = Guid.NewGuid(),
                            QuestionType = MapQuestionType(qData.Type),
                            RequiresAiAnalysis = true,
                            IsActive = true,
                            StepId = surveyStep.Id,
                            SortOrder = questionSortOrder++,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        _context.SurveyQuestions.Add(question);

                        // Add translations
                        foreach (var (langCode, text) in qData.Text)
                        {
                            if (languages.TryGetValue(langCode, out var langId))
                            {
                                _context.SurveyQuestionTranslations.Add(new SurveyQuestionTranslation
                                {
                                    Id = Guid.NewGuid(),
                                    QuestionId = question.Id,
                                    LanguageId = langId,
                                    Text = text,
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow
                                });
                            }
                        }

                        // Add options if any
                        if (qData.Options != null && qData.Options.Any())
                        {
                            // Assuming options are parallel arrays across languages
                            var enOptions = qData.Options.GetValueOrDefault("en") ?? qData.Options.Values.First();
                            
                            for (int i = 0; i < enOptions.Count; i++)
                            {
                                var option = new SurveyQuestionOption
                                {
                                    Id = Guid.NewGuid(),
                                    QuestionId = question.Id,
                                    SortOrder = i + 1,
                                    Value = enOptions[i].ToLower().Replace(" ", "_"),
                                    IsActive = true,
                                    CreatedAt = DateTime.UtcNow
                                };

                                _context.SurveyQuestionOptions.Add(option);

                                foreach (var (langCode, optionsList) in qData.Options)
                                {
                                    if (i < optionsList.Count && languages.TryGetValue(langCode, out var langId))
                                    {
                                        _context.SurveyQuestionOptionTranslations.Add(new SurveyQuestionOptionTranslation
                                        {
                                            Id = Guid.NewGuid(),
                                            OptionId = option.Id,
                                            LanguageId = langId,
                                            Text = optionsList[i],
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Surveys seeded successfully.");
        }

        private string MapQuestionType(string oldType)
        {
            return oldType.ToLower() switch
            {
                "select" => "single-choice",
                "radio" => "single-choice",
                "multiselect" => "multiple-choice",
                "checkbox" => "multiple-choice",
                "input" => "text",
                "textarea" => "text",
                "number" => "number",
                _ => "text"
            };
        }

        private string GetSurveyTitle(string type)
        {
            return type switch
            {
                "ab-test" => "A/B Test Survey",
                "intro" => "Parent Trial: My Career Orientation",
                "parent-child-talents" => "Parent View of Child's Talents",
                "parent" => "Parent Survey (Full Version)",
                _ => "Survey"
            };
        }

        private string GetSurveyDescription(string type)
        {
            return type switch
            {
                "ab-test" => "Discover your future professions with our new question set, unlock badges, and get personalized advice powered by AI.",
                "intro" => "Short survey for parents. Learn more about your career orientation, motives, and expectations.",
                "parent-child-talents" => "Describe your child's talents, interests, and characteristics for personalized career recommendations.",
                "parent" => "Comprehensive survey for parents: parent profile, attitude towards child, expectations from the tool.",
                _ => ""
            };
        }
    }
}
