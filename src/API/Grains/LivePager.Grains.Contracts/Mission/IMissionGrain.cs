using LivePager.Grains.Contracts.Participant;

namespace LivePager.Grains.Contracts.Mission
{
    public interface IMissionGrain : IGrainWithGuidKey
    {
        Task SetUserLocationAsync(LocationDataPoint dataPoint);
    }
}
