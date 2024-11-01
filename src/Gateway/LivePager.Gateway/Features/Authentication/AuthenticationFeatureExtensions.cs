using LivePager.Gateway.Features.Authentication.CreateUser.Logic;
using LivePager.Gateway.Features.Users;

namespace LivePager.Gateway.Features.Authentication
{
    public static class AuthenticationFeatureExtensions
    {
        public static IServiceCollection AddAuthenticationFeature(
            this IServiceCollection services)
        {
            return services
                .AddTransient<CreateUserService>()
                .AddSingleton<PasswordHasher>()
                .AddTransient<AuthenticationService>();
        }

        public static WebApplication UseAuthenticationFeature(
            this WebApplication webApplication)
        {
            var authenticationGroup = webApplication
                .MapGroup("/api/authentication")
                .WithOpenApi();

            authenticationGroup
                .MapPost("/login", AuthenticationEndpoints.Login)
                .AllowAnonymous();

            return webApplication;
        }
    }
}
