namespace LivePager.Gateway.Features.Mission.Requests
{
    public sealed class CreateMissionRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Longitude { get; set; }
        public required decimal Latitude { get; set; }
        public required decimal SearchRadius { get; set; }
    }
}
