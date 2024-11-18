using LivePager.Gateway.Features.Authentication.Contracts.CreateUser;
using LivePager.Gateway.Features.Authentication.CreateUser;
using LivePager.Gateway.Features.Authentication.Requests;
using LivePager.Gateway.Features.Authentication.Responses;
using LivePager.Gateway.Features.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LivePager.Gateway.Features.Authentication
{
    internal class AuthenticationEndpoints
    {
        internal static async Task<Results<Ok<CreateUserResponse>, BadRequest<string>>> CreateUser(
            [FromBody] CreateUserRequest request,
            [FromServices] CreateUserServiceOrchestrator serviceOrchestrator,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await serviceOrchestrator
                    .ExecuteAsync(request);

                return TypedResults.Ok(response);
            }
            catch (ValidationException ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

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