using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class SurveyResultConfiguration : IEntityTypeConfiguration<SurveyResult>
    {
        public void Configure(EntityTypeBuilder<SurveyResult> builder)
        {
            builder.HasKey(e => e.Id);
            
            // Relationships
            // Note: Using NoAction for Member and Family to avoid multiple cascade paths in SQL Server
            builder.HasOne(e => e.Survey)
                .WithMany(s => s.SurveyResults)
                .HasForeignKey(e => e.SurveyId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasOne(e => e.User)
                .WithMany(u => u.SurveyResults)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
                
            builder.HasOne(e => e.Member)
                .WithMany(m => m.SurveyResults)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.NoAction);
                
            builder.HasOne(e => e.Family)
                .WithMany(f => f.SurveyResults)
                .HasForeignKey(e => e.FamilyId)
                .OnDelete(DeleteBehavior.NoAction);
                
            builder.HasOne(e => e.Language)
                .WithMany()
                .HasForeignKey(e => e.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Indexes
            builder.HasIndex(e => e.SurveyId);
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.MemberId);
            builder.HasIndex(e => e.FamilyId);
            builder.HasIndex(e => e.LanguageId);
            builder.HasIndex(e => e.SurveyType);
            builder.HasIndex(e => e.CurrentStep);
            builder.HasIndex(e => e.UpdatedAt);
            builder.HasIndex(e => e.CompletedAt);
        }
    }
}
