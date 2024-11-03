namespace LivePager.Gateway.Features.Authentication.CreateUser.Logic
{
    public sealed record CreateUserResult
    {
        public bool Success { get; set; }
        public Dictionary<string, List<string>>? ValidationErrors { get; set; }
        public UserObject? User { get; set; }

        public sealed record UserObject
        {
            public required string Email { get; init; }
            public required string Username { get; init; }
            public required string HashedPassword { get; init; }
            public string? DisplayName { get; init; }
        }
    }
}
