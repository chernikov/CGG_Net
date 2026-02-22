using Microsoft.Extensions.DependencyInjection;

namespace CGG.Core;

public static class Dependencies
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // Core layer typically contains domain entities and interfaces only
        // No services to register at this level
        
        return services;
    }
}
