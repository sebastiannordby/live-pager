namespace LivePager.Gateway.Features.Mission.FindMission
{
    public class FindMissionResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Organization { get; internal set; }
        public decimal Longitude { get; internal set; }
        public decimal Latitude { get; internal set; }
        public decimal SearchRadius { get; internal set; }
    }
}
