namespace LivePager.Grains.Contracts.Participant
{
    [Serializable]
    public class LocationState
    {
        public List<LocationDataPoint> DataPoints { get; set; } = new List<LocationDataPoint>();
    }
}
