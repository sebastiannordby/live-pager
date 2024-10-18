namespace LivePager.API.Features.Location.Requests
{
    public class AddLocationDataPointRequest
    {
        public required decimal Longitude { get; set; }
        public required decimal Latitude { get; set; }
        public required string UserIdentificator { get; set; }
    }
}
