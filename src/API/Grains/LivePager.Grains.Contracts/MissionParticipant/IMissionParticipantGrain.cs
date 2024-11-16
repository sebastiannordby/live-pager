namespace LivePager.Grains.Contracts.MissionParticipant
{
    public interface IMissionParticipantGrain : IGrainWithStringKey
    {
        Task AddLocationAsync(LocationDataPoint dataPoint);

        Task InitializeAsync(string emailAddress, string displayName, Guid missionId);

        Task<LocationDataPoint[]> GetDataPointsAsync();
    }
}
