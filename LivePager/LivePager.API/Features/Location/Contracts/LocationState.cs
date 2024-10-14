namespace LivePager.API.Features.Location.Contracts
{
    [Serializable]
    public class LocationState
    {
        public List<LocationDataPoint> DataPoints { get; set; } = new List<LocationDataPoint>();
    }
}
