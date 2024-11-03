using LivePager.Gateway.Infrastructure;
using LivePager.Gateway.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace LivePager.Gateway.Features.Users
{
    public sealed class AuthenticationService
    {
        public LiverPagerDbContext _dbContext;

        public AuthenticationService(LiverPagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static string HashPassword(
            string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public async Task<User?> ValidateUserAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default)
        {
            var hashedPassword = HashPassword(password);
            var user = await _dbContext.Users
                .Where(x => x.Username == username)
                .Where(x => x.PasswordHash == hashedPassword)
                .FirstOrDefaultAsync(cancellationToken);

            return user;
        }
    }
}
