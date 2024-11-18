namespace LivePager.Gateway.Features.Authentication.Contracts.CreateUser
{
    public sealed class CreateUserRequest
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public string? DisplayName { get; set; }
        public required string Password { get; set; }
    }
}
