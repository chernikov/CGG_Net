using CGG.Application.Interfaces;
using CGG.Application.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;

namespace CGG.Application;

public static class Dependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(Dependencies).Assembly);

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Dependencies).Assembly));

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(Dependencies).Assembly);

        // Application Services
        services.AddScoped<IAuthService, AuthService>();

        // Email Service with Resend
        var resendApiKey = configuration["Resend:ApiKey"];
        if (!string.IsNullOrEmpty(resendApiKey))
        {
            services.AddOptions<ResendClientOptions>().Configure(o =>
            {
                o.ApiToken = resendApiKey;
            });
            services.AddTransient<IResend, ResendClient>();
            services.AddScoped<IEmailService, EmailService>();
        }
        else
        {
            // Fallback to console logger if no API key
            services.AddScoped<IEmailService, EmailService>();
        }

        return services;
    }
}
