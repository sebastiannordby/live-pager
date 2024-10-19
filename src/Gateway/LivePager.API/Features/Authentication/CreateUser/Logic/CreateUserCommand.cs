﻿namespace LivePager.API.Features.Authentication.CreateUser.Logic
{
    public class CreateUserCommand
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}