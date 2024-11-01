using LivePager.Grains.Contracts.Participant;

namespace LivePager.Grains.Contracts.Mission
{
    public interface IMissionGrain : IGrainWithGuidKey
    {
        Task CreateMissionAsync(
            string name,
            string? description,
            decimal longitude,
            decimal latitude,
            decimal searchRadius);

        Task<MissionState> GetMissionStateAsync();

        Task SetUserLocationAsync(LocationDataPoint dataPoint);
    }
}
