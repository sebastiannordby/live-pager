namespace LivePager.Grains.Contracts.Participant
{
    public interface IParticipantGrain : IGrainWithStringKey
    {
        ValueTask AddLocationAsync(
            LocationDataPoint dataPoint);
        ValueTask<LocationDataPoint[]> GetDataPointsAsync();
    }
}
