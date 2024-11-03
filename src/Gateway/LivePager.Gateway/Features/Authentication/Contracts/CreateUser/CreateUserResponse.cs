namespace LivePager.Gateway.Features.Authentication.Contracts.CreateUser
{
    public sealed record CreateUserResponse
    {
        public required Guid Id { get; init; }
        public required string Username { get; init; }
    }
}
