using LivePager.Gateway.Features.Mission.Requests;
using LivePager.Gateway.Features.Mission.Responses;
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

        internal static async Task<Ok<GetMissionsResponse>> GetMissions(
            [FromServices] IGrainFactory grainFactory)
        {
            var missionCollectionGrain = grainFactory
                .GetGrain<IMissionCollectionGrain>("GlobalMissionCollection");

            var missionNames = await missionCollectionGrain
                .GetMissions();

            var missions = missionNames.Select(mission =>
                new GetMissionsResponseMissionDto()
                {
                    Name = mission.Name,
                    Created = mission.Created,
                    Updated = mission.Updated,
                    Organization = mission.Organization
                }).ToArray();

            var response = new GetMissionsResponse()
            {
                Missions = missions
            };

            return TypedResults.Ok(response);
        }
    }
}
