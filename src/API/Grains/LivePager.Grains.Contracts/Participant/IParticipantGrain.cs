namespace LivePager.Grains.Contracts.Participant
{
    public interface IParticipantGrain : IGrainWithStringKey
    {
        Task AddLocationAsync(LocationDataPoint dataPoint);

        Task<LocationDataPoint[]> GetDataPointsAsync();
    }
}
