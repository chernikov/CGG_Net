using CGG.Core.Entities;
using CGG.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CGG.Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public DataSeeder(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roles = new[]
            {
                new { Name = "admin",        Description = "System administrator with full access" },
                new { Name = "student",      Description = "Student user" },
                new { Name = "child",        Description = "Child member of a family" },
                new { Name = "parent",       Description = "Parent member of a family" },
                new { Name = "teacher",      Description = "Teacher at a school" },
                new { Name = "school-admin", Description = "School administrator" },
            };

            foreach (var r in roles)
            {
                if (!await _context.Roles.AnyAsync(x => x.Name == r.Name))
                {
                    _context.Roles.Add(new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = r.Name,
                        Description = r.Description,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedAdminUserAsync()
        {
            const string adminEmail = "admin@careergg.com";

            if (await _context.Set<User>().AnyAsync(u => u.Email == adminEmail))
                return;

            var admin = new User
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                DisplayName = "Administrator",
                Role = UserRole.Admin,
                EmailConfirmed = true,
                Credits = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = string.Empty
            };

            admin.PasswordHash = _passwordHasher.Hash("Admin123!");

            _context.Set<User>().Add(admin);
            await _context.SaveChangesAsync();
        }
    }
}
