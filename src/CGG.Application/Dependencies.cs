using CGG.Application.Interfaces;
using CGG.Application.Services;
using CGG.Core.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
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

        // Password Hasher
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        // Application Services
        services.AddScoped<IAuthService, AuthService>();

        // Email Service with Resend
        var resendApiKey = configuration["Resend:ApiKey"] 
            ?? throw new InvalidOperationException("Resend:ApiKey is not configured");
        
        services.AddOptions<ResendClientOptions>()
            .Configure(options => options.ApiToken = resendApiKey);
        
        services.AddHttpClient<IResend, ResendClient>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
