using Microsoft.Extensions.DependencyInjection;

namespace CGG.Application;

public static class Dependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(Dependencies).Assembly);

        // FluentValidation
        // services.AddValidatorsFromAssembly(typeof(Dependencies).Assembly);

        // TODO: Add application services
        // services.AddScoped<ISurveyService, SurveyService>();
        // services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
