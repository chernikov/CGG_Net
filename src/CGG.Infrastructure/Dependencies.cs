using CGG.Core.Entities;
using CGG.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CGG.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Identity (AddIdentity includes SignInManager which requires ASP.NET Core)
        // For class library, we configure Identity through the API project
        // services.AddIdentityCore<User>()
        //     .AddRoles<IdentityRole<Guid>>()
        //     .AddEntityFrameworkStores<ApplicationDbContext>();

        // TODO: Add repositories, email service, etc.
        // services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
