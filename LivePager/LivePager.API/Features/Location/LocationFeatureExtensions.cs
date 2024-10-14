namespace LivePager.API.Features.Location
{
    public static class LocationFeatureExtensions
    {
        public static WebApplicationBuilder AddLocationFeature(
            this WebApplicationBuilder builder)
        {
            return builder;
        }

        public static WebApplication UseLocationFeature(
            this WebApplication webApplication)
        {
            webApplication
                .MapHub<LocationHub>("/location-hub");

            var locationGroup = webApplication
                .MapGroup("location")
                .WithOpenApi();

            locationGroup.MapPost(
                string.Empty, LocationEndpoints.AddLocationDataPoint);

            return webApplication;
        }
    }
}
