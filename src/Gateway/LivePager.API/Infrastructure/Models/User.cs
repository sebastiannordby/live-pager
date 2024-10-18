﻿namespace LivePager.API.Infrastructure.Models
{
    public sealed class User
    {
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
    }
}
