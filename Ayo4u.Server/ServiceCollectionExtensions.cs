using Ayo4u.Data;
using Ayo4u.Server.Shared;

namespace Ayo4u.Server
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedInfrastructure();

            services.AddAyoDbContext(configuration);

            services.AddDataRepositories();

            return services;
        }
    }
}
