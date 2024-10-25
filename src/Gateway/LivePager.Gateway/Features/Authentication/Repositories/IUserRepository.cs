using LivePager.Gateway.Infrastructure.Models;

namespace LivePager.Gateway.Features.Authentication.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindAsync(string username, string password);
    }
}
