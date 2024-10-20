﻿
using LivePager.API.Features.Authentication.Requests;
using LivePager.API.Features.Authentication.Responses;
using LivePager.API.Features.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LivePager.API.Features.Authentication
{
    internal class AuthenticationEndpoints
    {
        internal static async Task<Results<Ok<LivePagerLoginResponse>, UnauthorizedHttpResult>> Login(
            [FromBody] LivePagerLoginRequest request,
            [FromServices] AuthenticationService service,
            [FromServices] IConfiguration configuration,
            CancellationToken cancellationToken = default)
        {
            var user = await service.ValidateUserAsync(
                request.Username, request.Password, cancellationToken);

            if (user == null)
            {
                return TypedResults.Unauthorized();
            }

            var token = JwtTokenGenerator
                .GenerateToken(user.Username, configuration["Keys:Authentication:Secret"]!);
            var loginResponse = new LivePagerLoginResponse()
            {
                JwtToken = token
            };

            return TypedResults.Ok(loginResponse);
        }
    }
}