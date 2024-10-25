namespace LivePager.Gateway.Features.Authentication.CreateUser.Logic
{
    public class CreateUserResult
    {
        public bool Success { get; set; }
        public Dictionary<string, List<string>>? ValidationErrors { get; set; }

        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? HashedPassword { get; set; }
    }
}
