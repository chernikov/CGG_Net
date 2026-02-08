using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class SurveyQuestionOptionTranslationConfiguration : IEntityTypeConfiguration<SurveyQuestionOptionTranslation>
    {
        public void Configure(EntityTypeBuilder<SurveyQuestionOptionTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.OptionId, e.LanguageId }).IsUnique();
            
            builder.HasOne(e => e.Option)
                .WithMany(o => o.Translations)
                .HasForeignKey(e => e.OptionId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(e => e.Language)
                .WithMany(l => l.OptionTranslations)
                .HasForeignKey(e => e.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
