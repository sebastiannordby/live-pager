﻿
namespace LivePager.Gateway.Infrastructure.Models
{
    public sealed class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Email { get; set; }
        public string? DisplayName { get; set; }
    }
}
