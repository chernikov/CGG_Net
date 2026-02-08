using CGG.Core.Entities;
using CGG.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CGG.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Family> Families { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<School> Schools { get; set; }
        
        // Survey-related tables
        public DbSet<Language> Languages { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyStep> SurveySteps { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyQuestionTranslation> SurveyQuestionTranslations { get; set; }
        public DbSet<SurveyQuestionOption> SurveyQuestionOptions { get; set; }
        public DbSet<SurveyQuestionOptionTranslation> SurveyQuestionOptionTranslations { get; set; }
        public DbSet<SurveyResult> SurveyResults { get; set; }
        
        // AI-related tables
        public DbSet<AiPromptTemplate> AiPromptTemplates { get; set; }
        public DbSet<AiLog> AiLogs { get; set; }
        
        public DbSet<AIRecommendation> AIRecommendations { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all entity configurations
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new FamilyConfiguration());
            builder.ApplyConfiguration(new MemberConfiguration());
            builder.ApplyConfiguration(new TransactionConfiguration());
            builder.ApplyConfiguration(new SchoolConfiguration());
            builder.ApplyConfiguration(new PromoCodeConfiguration());
            builder.ApplyConfiguration(new LanguageConfiguration());
            builder.ApplyConfiguration(new SurveyConfiguration());
            builder.ApplyConfiguration(new SurveyStepConfiguration());
            builder.ApplyConfiguration(new SurveyQuestionConfiguration());
            builder.ApplyConfiguration(new SurveyQuestionTranslationConfiguration());
            builder.ApplyConfiguration(new SurveyQuestionOptionConfiguration());
            builder.ApplyConfiguration(new SurveyQuestionOptionTranslationConfiguration());
            builder.ApplyConfiguration(new SurveyResultConfiguration());
            builder.ApplyConfiguration(new AIRecommendationConfiguration());
            builder.ApplyConfiguration(new AiPromptTemplateConfiguration());
            builder.ApplyConfiguration(new AiLogConfiguration());
        }
    }
}
