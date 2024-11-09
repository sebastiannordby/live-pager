using LivePager.Gateway.Features.Authentication.Repositories;
using LivePager.Gateway.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LivePager.Gateway.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration["Secrets:ConnectionString"]))
                throw new ArgumentException("Secrets:ConnectionString not provided.");

            services.AddDbContext<LiverPagerDbContext>(options =>
            {
                var connectionString = configuration["Secrets:ConnectionString"];

                options.UseSqlServer(
                    connectionString);
            });

            return services
                .AddTransient<IUserRepository, UserRepository>();
        }
    }
}
