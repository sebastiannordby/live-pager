using LivePager.Gateway.Features.Authentication.Repositories;
using LivePager.Gateway.Infrastructure.Models;

namespace LivePager.Gateway.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> FindAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
