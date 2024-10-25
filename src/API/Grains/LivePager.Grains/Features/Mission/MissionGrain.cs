using LivePager.Grains.Contracts.Mission;
using LivePager.Grains.Contracts.Participant;
using Orleans.Providers;
using Orleans.Streams;

namespace LivePager.Grains.Features.Mission
{
    [StorageProvider(ProviderName = "MissionStore")]
    public sealed class MissionGrain : Grain<MissionState>, IMissionGrain
    {
        private IStreamProvider _streamProvider = null!;
        private IAsyncStream<LocationDataPoint> _stream = null!;

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
