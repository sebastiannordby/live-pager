using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LivePager.Gateway.Infrastructure
{
    public class LiverPagerDbContextFactory : IDesignTimeDbContextFactory<LiverPagerDbContext>
    {
        public LiverPagerDbContext CreateDbContext(string[] args)
        {
            // Build the configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<LiverPagerDbContext>();

            // Use the connection string from appsettings.json
            optionsBuilder.UseSqlServer(configuration["Secrets:ConnectionString"]);

            return new LiverPagerDbContext(optionsBuilder.Options);
        }
    }
}
