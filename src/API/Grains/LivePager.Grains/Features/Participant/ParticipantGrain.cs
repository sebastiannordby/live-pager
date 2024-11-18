using LivePager.Grains.Contracts;
using LivePager.Grains.Contracts.MissionParticipant;
using Orleans.Providers;
using Orleans.Streams;

namespace LivePager.Grains.Features.Participant
{
    [StorageProvider(ProviderName = "LocationStore")]
    public class ParticipantGrain : Grain<ParticipantState>, IMissionParticipantGrain
    {
        private IAsyncStream<LocationDataPoint> _locationStream = null!;

        public async Task AddLocationAsync(
            LocationDataPoint dataPoint)
        {
            if (State.DataPoints.Any(x => x.Longitude == dataPoint.Longitude
                && x.Latitude == dataPoint.Latitude))
            {
                return;
            }

            State.DataPoints.Add(dataPoint);

            await _locationStream.OnNextAsync(dataPoint);

            await WriteStateAsync();
        }

        public async Task<LocationDataPoint[]> GetDataPointsAsync()
        {
            return await Task.FromResult(State.DataPoints.ToArray());
        }

        public async Task InitializeAsync(
            string emailAddress, string displayName, Guid missionId)
        {
            State.EmailAddress = emailAddress;
            State.DisplayName = displayName;
            State.MissionId = missionId;
            await WriteStateAsync();
            InitializeStream();
        }

        private void InitializeStream()
        {
            var streamProvider = this.GetStreamProvider(
                LivePagerOrleansConstants.DefaultStreamProvider);
            var streamId = StreamId.Create(
                "participant_location", this.State.MissionId);

            _locationStream = streamProvider
                .GetStream<LocationDataPoint>(streamId);
        }
    }
}
