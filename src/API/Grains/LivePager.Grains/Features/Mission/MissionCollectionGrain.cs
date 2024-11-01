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

        public async Task<string[]> GetMissions()
        {
            var missionNames = State.Missions
                .Select(x => x.Value)
                .ToArray();

            return await Task.FromResult(missionNames);
        }
    }
}
