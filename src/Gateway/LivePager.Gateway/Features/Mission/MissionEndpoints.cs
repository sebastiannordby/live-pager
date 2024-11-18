using LivePager.Gateway.Features.Mission.Contracts.CreateMission;
using LivePager.Gateway.Features.Mission.Contracts.FindMission;
using LivePager.Gateway.Features.Mission.Contracts.GetMissions;
using LivePager.Gateway.Features.Mission.Contracts.RegisterLocation;
using LivePager.Grains.Contracts.Mission;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LivePager.Gateway.Features.Mission
{
    internal class MissionEndpoints
    {
        internal static async Task<Ok> RegisterLocationDataPoint(
            [FromBody] RegisterParticipantLocationRequest request,
            [FromServices] IGrainFactory grainFactory,
            CancellationToken cancellationToken)
        {
            var missionGrain = grainFactory
                .GetGrain<IMissionGrain>(request.MissionId);

            await missionGrain.SetUserLocationAsync(new()
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude,
            });

            return TypedResults.Ok();
        }

        internal static async Task<Ok> CreateMission(
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

        internal static async Task<Ok<FindMissionResponse>> FindMission(
            [FromRoute] Guid missionId,
            [FromServices] IGrainFactory grainFactory)
        {
            var missionGrain = grainFactory
                .GetGrain<IMissionGrain>(missionId);

            var missionState = await missionGrain
                .GetMissionStateAsync();

            var response = new FindMissionResponse()
            {
                Id = missionId,
                Name = missionState.Name,
                Description = missionState.Description,
                Organization = missionState.Organization,
                Longitude = missionState.Longitude,
                Latitude = missionState.Latitude,
                SearchRadius = missionState.SearchRadius
            };

            return TypedResults.Ok(response);
        }

        internal static async Task<Ok<GetMissionsResponse>> GetMissions(
            [FromServices] IGrainFactory grainFactory)
        {
            var missionCollectionGrain = grainFactory
                .GetGrain<IMissionCollectionGrain>("GlobalMissionCollection");

            var missionNames = await missionCollectionGrain
                .GetMissionsAsync();

            var missions = missionNames.Select(mission =>
                new GetMissionsResponseMissionDto()
                {
                    Id = mission.Id,
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
