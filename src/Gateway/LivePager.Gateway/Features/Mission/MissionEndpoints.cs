using LivePager.Gateway.Features.Mission.Requests;
using LivePager.Grains.Contracts.Mission;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LivePager.Gateway.Features.Mission
{
    public class MissionEndpoints
    {
        public static async Task<Ok> CreateMission(
            [FromBody] CreateMissionRequest request,
            [FromServices] IGrainFactory grainFactory)
        {
            var missionGrain = grainFactory
                .GetGrain<IMissionGrain>(Guid.NewGuid());

            await missionGrain.CreateMissionAsync(
                name: request.Name,
                description: request.Description,
                longitude: request.Longitude,
                latitude: request.Latitude,
                searchRadius: request.SearchRadius);

            return TypedResults.Ok();
        }
    }
}
