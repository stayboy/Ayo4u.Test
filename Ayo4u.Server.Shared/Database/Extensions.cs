using Microsoft.Extensions.DependencyInjection;

namespace Server.Shared.Database;

internal static class Extensions
{
    internal static IServiceCollection AddPostgresMigration(this IServiceCollection services)
    {
        services.AddHostedService<DbAppInitializer>();

        return services;
    }
}
