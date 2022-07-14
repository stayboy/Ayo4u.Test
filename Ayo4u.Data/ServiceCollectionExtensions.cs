using Ayo4u.Data.Repositories;
using Ayo4u.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ayo4u.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAyoDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAyoPostgresContext<AyoDbContext>(configuration);

            return services;
        }

        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            services.TryAddScoped<IUserRepository, UserRepository>();
            services.TryAddScoped<IConverterRepository, UnitConverterRepository>();
            services.TryAddScoped<IRequestActionRepository, RequestActionRepository>();

            return services;
        }
    }
}