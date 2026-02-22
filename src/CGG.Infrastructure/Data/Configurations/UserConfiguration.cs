using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(e => e.FirstName)
                .HasMaxLength(50);

            builder.Property(e => e.Surname)
                .HasMaxLength(50);

            builder.Property(e => e.Age);

            builder.Property(e => e.AgeAddedDate);

            builder.Property(e => e.Role)
                .IsRequired();

            builder.Property(e => e.Credits)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.EmailConfirmed)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(e => e.Family)
                .WithMany()
                .HasForeignKey(e => e.FamilyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.Member)
                .WithOne()
                .HasForeignKey<User>(e => e.MemberId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.School)
                .WithMany(s => s.Users)
                .HasForeignKey(e => e.SchoolId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.HasIndex(e => e.FamilyId);
            builder.HasIndex(e => e.MemberId);
            builder.HasIndex(e => e.SchoolId);
            builder.HasIndex(e => e.Role);
            builder.HasIndex(e => e.CreatedAt);
        }
    }
}
