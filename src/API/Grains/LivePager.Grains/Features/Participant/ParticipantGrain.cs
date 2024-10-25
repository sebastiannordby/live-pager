using LivePager.Grains.Contracts.Participant;
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

        //private async Task PersistLocationAsync(
        //    object? arg,
        //    CancellationToken cancellationToken)
        //{
        //    //await _locationRepository.SaveLocationsAsync(State.DataPoints);
        //    State.DataPoints.Clear();
        //}

        public async ValueTask AddLocationAsync(
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

        public async ValueTask<LocationDataPoint[]> GetDataPointsAsync()
        {
            return await Task.FromResult(State.DataPoints.ToArray());
        }
    }
}
