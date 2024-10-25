namespace LivePager.Grains.Contracts.Location
{
    [Serializable]
    public class LocationState
    {
        public List<LocationDataPoint> DataPoints { get; set; } = new List<LocationDataPoint>();
    }
}
