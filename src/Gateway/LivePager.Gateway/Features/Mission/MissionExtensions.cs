namespace LivePager.Gateway.Features.Mission
{
    internal static class MissionExtensions
    {
        internal static WebApplication UseMissionEndpoints(this WebApplication webApp)
        {
            webApp.MapHub<ParticipantHub>("/mission-hub");

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
