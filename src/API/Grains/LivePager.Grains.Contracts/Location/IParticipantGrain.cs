using Orleans;

namespace LivePager.Grains.Contracts.Location
{
    public interface IParticipantGrain : IGrainWithStringKey
    {
        Task AddLocationAsync(
            LocationDataPoint dataPoint);
        Task<LocationDataPoint[]> GetDataPointsAsync();
    }
}
