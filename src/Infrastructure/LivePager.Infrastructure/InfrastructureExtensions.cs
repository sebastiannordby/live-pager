using LivePager.Grains.Features.Location.Repositories;
using LivePager.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LivePager.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddLivePagerInfrastructure(
            this IServiceCollection services)
        {
            return services
                .AddTransient<ILocationRepository, LocationRepository>();
        }
    }
}
