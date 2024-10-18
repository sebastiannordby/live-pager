using LivePager.API.Features.Users;

namespace LivePager.API.Features.Authentication
{
    public static class AuthenticationFeatureExtensions
    {
        public static IServiceCollection AddAuthenticationFeature(
            this IServiceCollection services)
        {
            return services
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
