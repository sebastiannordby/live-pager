using LivePager.Grains.Contracts.Mission;
using LivePager.Grains.Contracts.MissionParticipant;
using Orleans.Providers;
using Orleans.Streams;

namespace LivePager.Grains.Features.Mission
{
    [StorageProvider(ProviderName = "MissionStore")]
    public sealed class MissionGrain : Grain<MissionState>, IMissionGrain
    {
        private IStreamProvider _streamProvider = null!;
        private IAsyncStream<LocationDataPoint> _stream = null!;
        //private readonly IMissionRepository _missionRepository;

        //public MissionGrain(IMissionRepository missionRepository)
        //{
        //    _missionRepository = missionRepository;
        //}

        public async Task CreateMissionAsync(
            string name,
            string? description,
            decimal longitude,
            decimal latitude,
            decimal searchRadius)
        {
            State.Id = this.GetGrainId().GetGuidKey();
            State.Name = name;
            State.Description = description;
            State.Longitude = longitude;
            State.Latitude = latitude;
            State.SearchRadius = searchRadius;
            await WriteStateAsync();

            var missionCollectionGrain = GrainFactory
                .GetGrain<IMissionCollectionGrain>("GlobalMissionCollection");

            await missionCollectionGrain
                .AddMission(State.Id, State.Name);
        }

        public async Task<MissionState> GetMissionStateAsync()
        {
            return await Task.FromResult(this.State);
        }

        public override async Task OnActivateAsync(
            CancellationToken cancellationToken)
        {
            _streamProvider = this.GetStreamProvider("DefaultStreamProvider");
            _stream = _streamProvider.GetStream<LocationDataPoint>("LocationUpdates");

            await base.OnActivateAsync(cancellationToken);
        }

        public async Task SetUserLocationAsync(
            LocationDataPoint dataPoint)
        {
            await _stream.OnNextAsync(dataPoint);
        }
    }
}
