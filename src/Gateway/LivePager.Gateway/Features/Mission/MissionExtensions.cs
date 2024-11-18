namespace LivePager.Gateway.Features.Mission
{
    internal static class MissionExtensions
    {
        internal static WebApplicationBuilder AddMissionFeature(
            this WebApplicationBuilder webAppBuilder)
        {
            //webAppBuilder.Services
            //    .AddHostedService<MissionStreamHostedService>();

            return webAppBuilder;
        }

        internal static WebApplication UseMissionEndpoints(this WebApplication webApp)
        {
            webApp
                .MapHub<MissionSignalRHub>("/mission-hub")
                .RequireCors("frontend")
                .AllowAnonymous();

            var group = webApp
                .MapGroup("/api/mission")
                .WithOpenApi();

            group.MapGet("{missionId}", MissionEndpoints.FindMission);
            group.MapGet(string.Empty, MissionEndpoints.GetMissions);
            group.MapPost(string.Empty, MissionEndpoints.CreateMission);
            group.MapPost("/location", MissionEndpoints.RegisterLocationDataPoint);

            return webApp;
        }
    }
}
