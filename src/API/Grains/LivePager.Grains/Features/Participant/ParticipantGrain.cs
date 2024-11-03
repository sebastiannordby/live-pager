﻿using LivePager.Grains.Contracts.Participant;
using Orleans.Providers;
using Orleans.Streams;

namespace LivePager.Grains.Features.Participant
{
    [StorageProvider(ProviderName = "LocationStore")]
    public class ParticipantGrain : Grain<LocationState>, IParticipantGrain
    {
        private IAsyncStream<LocationDataPoint> _locationStream = null!;

        public override async Task OnActivateAsync(
            CancellationToken cancellationToken)
        {
            var streamProvider = this.GetStreamProvider("Default");
            _locationStream = streamProvider
                .GetStream<LocationDataPoint>(this.GetPrimaryKey());
        }

        public async Task AddLocationAsync(
            LocationDataPoint dataPoint)
        {
            if (State.DataPoints.Any(x => x.Longitude == dataPoint.Longitude
                && x.Latitude == dataPoint.Latitude))
            {
                return;
            }

            State.DataPoints.Add(dataPoint);

            await WriteStateAsync();
        }

        public async Task<LocationDataPoint[]> GetDataPointsAsync()
        {
            return await Task.FromResult(State.DataPoints.ToArray());
        }
    }
}
