using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class AiLogConfiguration : IEntityTypeConfiguration<AiLog>
    {
        public void Configure(EntityTypeBuilder<AiLog> builder)
        {
            builder.HasKey(e => e.Id);
            
            // Relationships
            // Note: Using NoAction for User and Member to avoid multiple cascade paths in SQL Server
            builder.HasOne(e => e.User)
                .WithMany(u => u.AiLogs)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
                
            builder.HasOne(e => e.Member)
                .WithMany(m => m.AiLogs)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.NoAction);
                
            builder.HasOne(e => e.SurveyResult)
                .WithMany(sr => sr.AiLogs)
                .HasForeignKey(e => e.SurveyResultId)
                .OnDelete(DeleteBehavior.NoAction);
                
            builder.HasOne(e => e.UserSurvey)
                .WithMany(us => us.AiLogs)
                .HasForeignKey(e => e.UserSurveyId)
                .OnDelete(DeleteBehavior.NoAction);
                
            builder.HasOne(e => e.PromptTemplate)
                .WithMany(pt => pt.AiLogs)
                .HasForeignKey(e => e.PromptTemplateId)
                .OnDelete(DeleteBehavior.NoAction);
            
            // Indexes
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.MemberId);
            builder.HasIndex(e => e.SurveyResultId);
            builder.HasIndex(e => e.UserSurveyId);
            builder.HasIndex(e => e.PromptTemplateId);
            builder.HasIndex(e => e.RequestType);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.CreatedAt);
            builder.HasIndex(e => new { e.UserId, e.CreatedAt });
            builder.HasIndex(e => new { e.MemberId, e.CreatedAt });
            
            // Decimal precision
            builder.Property(e => e.CostUsd).HasColumnType("decimal(18,6)");
        }
    }
}
