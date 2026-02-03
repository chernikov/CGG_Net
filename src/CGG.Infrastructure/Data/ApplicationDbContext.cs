using CGG.Core.Entities;
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
        public DbSet<SurveyResult> SurveyResults { get; set; }
        public DbSet<AIRecommendation> AIRecommendations { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.Credits).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Family)
                    .WithMany()
                    .HasForeignKey(e => e.FamilyId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<Family>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Credits).HasColumnType("decimal(18,2)");
            });

            builder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Member)
                    .HasForeignKey<Member>(e => e.UserId);
                entity.HasOne(e => e.Family)
                    .WithMany(f => f.Members)
                    .HasForeignKey(e => e.FamilyId);
            });

            builder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            });

            builder.Entity<School>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Credits).HasColumnType("decimal(18,2)");
            });

            builder.Entity<PromoCode>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Credits).HasColumnType("decimal(18,2)");
            });
        }
    }
}
