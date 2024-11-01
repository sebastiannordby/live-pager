using LivePager.Grains.Contracts.Mission;
using Orleans.Providers;

namespace LivePager.Grains.Features.Mission
{
    [StorageProvider(ProviderName = "MissionCollectionStore")]
    public sealed class MissionCollectionGrain : Grain<MissionCollectionState>, IMissionCollectionGrain
    {
        public override async Task OnActivateAsync(
            CancellationToken cancellationToken)
        {
            await ReadStateAsync();
        }

        public async Task AddMission(Guid id, string name)
        {
            this.State.Missions.Add(id, name);
            await WriteStateAsync();
        }

        public async Task<MissionDto[]> GetMissions()
        {
            var missions = new MissionDto[State.Missions.Count];

            for (var i = 0; i < missions.Length; i++)
            {
                var missionGrain = GrainFactory
                    .GetGrain<IMissionGrain>(State.Missions.ElementAt(i).Key);
                var missionState = await missionGrain
                    .GetMissionStateAsync();

                var mission = new MissionDto()
                {
                    Name = missionState.Name,
                    Description = missionState.Description,
                    Latitude = missionState.Latitude,
                    Longitude = missionState.Longitude,
                    SearchRadius = missionState.SearchRadius,
                    Organization = missionState.Organization,
                    Created = missionState.CreatedDate,
                    Updated = missionState.LastUpdatedDate,
                };

                missions[i] = mission;
            }

            return await Task.FromResult(missions);
        }
    }
}
