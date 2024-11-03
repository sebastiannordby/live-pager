using LivePager.Gateway.Features.Authentication.CreateUser;
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
                .AddTransient<CreateUserServiceOrchestrator>()
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

            authenticationGroup
                .MapPost("/create-user", AuthenticationEndpoints.CreateUser)
                .AllowAnonymous();

            return webApplication;
        }
    }
}
