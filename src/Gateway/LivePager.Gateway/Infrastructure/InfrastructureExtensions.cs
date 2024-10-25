using LivePager.Gateway.Features.Authentication.Repositories;
using LivePager.Gateway.Infrastructure.Repositories;

namespace LivePager.Gateway.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddTransient<IUserRepository, UserRepository>();
        }
    }
}
