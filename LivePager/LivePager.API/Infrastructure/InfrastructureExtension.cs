using LivePager.API.Features.Location.Repositories;
using LivePager.API.Infrastructure.Repositories;

namespace LivePager.API.Infrastructure
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddTransient<ILocationRepository, LocationRepository>();
        }
    }
}
