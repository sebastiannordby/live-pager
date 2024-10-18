using Orleans;

namespace LivePager.Grains.Contracts.Location
{
    public interface ILocationGrain : IGrainWithStringKey
    {
        Task AddLocationAsync(
            LocationDataPoint dataPoint);
        Task<LocationDataPoint[]> GetDataPointsAsync();
    }
}
