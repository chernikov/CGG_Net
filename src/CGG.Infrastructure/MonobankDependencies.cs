using CGG.Application.Interfaces;
using CGG.Application.Settings;
using CGG.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CGG.Infrastructure;

internal static class MonobankDependencies
{
    internal static IServiceCollection AddMonobank(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MonobankSettings>(configuration.GetSection("Monobank"));

        services.AddTransient<MonobankAuthHandler>();

        services.AddHttpClient("monobank", client =>
            {
                client.BaseAddress = new Uri("https://api.monobank.ua");
            })
            .AddHttpMessageHandler<MonobankAuthHandler>();

        services.AddScoped<IMonobankService, MonobankService>();

        return services;
    }
}
