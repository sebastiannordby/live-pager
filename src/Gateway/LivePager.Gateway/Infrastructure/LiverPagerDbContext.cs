using LivePager.Gateway.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LivePager.Gateway.Infrastructure
{
    public class LiverPagerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public LiverPagerDbContext(
            DbContextOptions<LiverPagerDbContext> options) : base(options)
        {

        }
    }
}
