using LivePager.Grains.Contracts.Location;
using LivePager.Grains.Features.Participant.Repositories;
using Orleans;
using Orleans.Providers;

namespace LivePager.Grains.Features.Participant
{
    [StorageProvider(ProviderName = "LocationStore")]
    public class ParticipantGrain : Grain<LocationState>, IParticipantGrain
    {
        //private readonly IDisposable _persistenceTimer;
        //private readonly ILocationRepository _locationRepository;

        //public ParticipantGrain(
        //    ILocationRepository locationRepository) : base()
        //{
        //    _locationRepository = locationRepository;
        //    _persistenceTimer = this.RegisterGrainTimer<object>(
        //        PersistLocationAsync, null!, new Orleans.Runtime.GrainTimerCreationOptions()
        //        {
        //            DueTime = TimeSpan.Zero,
        //            KeepAlive = true,
        //            Period = TimeSpan.FromSeconds(5)
        //        });
        //}

        private async Task PersistLocationAsync(
            object? arg, 
            CancellationToken cancellationToken)
        {
            //await _locationRepository.SaveLocationsAsync(State.DataPoints);
            State.DataPoints.Clear();
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
