using LivePager.API.Infrastructure.Models;

namespace LivePager.API.Features.Authentication.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindAsync(string username, string password);
    }
}
