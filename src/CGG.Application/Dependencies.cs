using CGG.Application.Interfaces;
using CGG.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CGG.Application;

public static class Dependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(Dependencies).Assembly);

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Dependencies).Assembly));

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(Dependencies).Assembly);

        // Application Services
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
