using CGG.Application.Interfaces;
using CGG.Core.Interfaces;
using CGG.Infrastructure.Data;
using CGG.Infrastructure.Services;
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

        // Password Hasher
        services.AddScoped<IPasswordHasher, Security.PasswordHasher>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Generic repositories
        services.AddScoped(typeof(IReadRepository<>), typeof(Repositories.ReadRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repositories.Repository<>));

        // Domain-specific repositories
        services.AddScoped<IUserRepository, Repositories.UserRepository>();
        services.AddScoped<ISurveyRepository, Repositories.SurveyRepository>();
        services.AddScoped<IUserSurveyRepository, Repositories.UserSurveyRepository>();
        services.AddScoped<IUserSurveyAnswerRepository, Repositories.UserSurveyAnswerRepository>();

        // Infrastructure Services
        services.AddScoped<SurveySeeder>();
        services.AddScoped<PromptSeeder>();
        services.AddScoped<SurveyExampleSeeder>();
        services.AddScoped<ISurveyManagementService, SurveyManagementService>();

        // AI Services
        services.AddHttpClient();
        services.AddScoped<IAiPromptLoaderService, AiPromptLoaderService>();
        services.AddScoped<IAiSurveyService, AiSurveyService>();

        return services;
    }
}
