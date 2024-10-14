using LivePager.API.Features.Location.Contracts;
using LivePager.API.Features.Location.Repositories;
using Orleans.Providers;

namespace LivePager.API.Features.Location
{
    [StorageProvider(ProviderName = "LocationStore")]
    public class LocationGrain : Grain<LocationState>, IGrainWithStringKey, ILocationGrain
    {
        private const int MaxDataPointsInMemory = 100;
        private readonly ILocationRepository _locationRepository;

        public LocationGrain(
            ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
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

            if (State.DataPoints.Count >= MaxDataPointsInMemory)
            {
                await _locationRepository.SaveLocationsAsync(State.DataPoints);
                State.DataPoints.Clear();
            }

            await WriteStateAsync();
        }

        public async Task<LocationDataPoint[]> GetDataPointsAsync()
        {
            return await Task.FromResult(State.DataPoints.ToArray());
        }
    }
}
