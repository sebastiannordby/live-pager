using LivePager.Gateway.Features.Authentication.CreateUser;
using LivePager.Gateway.Infrastructure.Models;
using System.Security.Cryptography;
using System.Text;

namespace LivePager.Gateway.Features.Users
{
    public sealed class AuthenticationService
    {
        public static List<User> Users = new List<User>
        {
            new User
            {
                Username = "user1",
                PasswordHash = HashPassword("password1")
            }
        };

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
            var user = Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hashedPassword);

            return await Task.FromResult(user);
        }

        public async Task<User?> CreateUserAsync(
            CreateUserRequest request,
            CancellationToken cancellationToken = default)
        {
            return null;
        }
    }
}
