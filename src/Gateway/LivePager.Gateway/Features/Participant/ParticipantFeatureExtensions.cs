namespace LivePager.Gateway.Features.Location
{
    public static class ParticipantFeatureExtensions
    {
        public static WebApplication UseParticipantFeature(
            this WebApplication webApplication)
        {
            webApplication
                .MapHub<ParticipantHub>("/participant-hub");

            var routeGroup = webApplication
                .MapGroup("participant")
                .WithOpenApi();

            routeGroup.MapPost(
                string.Empty, ParticipantEndpoints.AddLocationDataPoint);

            return webApplication;
        }
    }
}
