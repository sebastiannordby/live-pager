using LivePager.Grains.Features.Mission.Repositories;
using LivePager.Grains.Features.Participant.Repositories;
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
                .AddSingleton<ILocationRepository, InMemoryLocationRepository>()
                .AddSingleton<IMissionRepository, InMemoryMissionRepository>();
        }
    }
}
