namespace LivePager.Grains.Contracts.Location
{
    public interface IParticipantGrain : IGrainWithStringKey
    {
        ValueTask AddLocationAsync(
            LocationDataPoint dataPoint);
        ValueTask<LocationDataPoint[]> GetDataPointsAsync();
    }
}
