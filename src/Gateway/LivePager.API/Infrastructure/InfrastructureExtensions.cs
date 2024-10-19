using LivePager.API.Features.Authentication.Repositories;
using LivePager.API.Infrastructure.Repositories;

namespace LivePager.API.Infrastructure
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
