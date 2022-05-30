using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ayo4u.Server.Shared.Database;

internal static class Extensions
{
    internal static IServiceCollection AddPostgresMigration(this IServiceCollection services)
    {
        services.AddHostedService<DbAppInitializer>();

        return services;
    }
}
