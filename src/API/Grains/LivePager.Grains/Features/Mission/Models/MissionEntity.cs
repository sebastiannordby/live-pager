namespace LivePager.Grains.Features.Mission.Models
{
    public sealed class MissionEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal SearchRadius { get; set; }
    }
}
