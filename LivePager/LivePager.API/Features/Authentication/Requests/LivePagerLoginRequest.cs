﻿namespace LivePager.API.Features.Authentication.Requests
{
    public class LivePagerLoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
