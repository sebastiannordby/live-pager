using LivePager.API.Features.Authentication.Repositories;
using LivePager.API.Infrastructure.Models;

namespace LivePager.API.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> FindAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
