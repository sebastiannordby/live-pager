namespace LivePager.API.Features.Authentication.CreateUser
{
    public sealed class CreateUserRequest
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
