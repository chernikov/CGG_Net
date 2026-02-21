using CGG.Core.Entities;
using CGG.Core.Interfaces;
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

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Generic repositories
        services.AddScoped(typeof(IReadRepository<>), typeof(Repositories.ReadRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repositories.Repository<>));

        // Domain-specific repositories
        services.AddScoped<IUserRepository, Repositories.UserRepository>();

        return services;
    }
}
