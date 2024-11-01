
namespace LivePager.Grains.Contracts.Mission
{
    [GenerateSerializer]
    public class MissionState
    {
        [Id(0)]
        public required string Name { get; set; }

        [Id(1)]
        public string? Description { get; set; }

        [Id(2)]
        public List<string> Participants { get; set; } = new();

        [Id(3)]
        public decimal Longitude { get; set; }

        [Id(4)]
        public decimal Latitude { get; set; }

        [Id(5)]
        public decimal SearchRadius { get; set; }
    }
}
