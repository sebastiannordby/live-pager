
namespace LivePager.Grains.Contracts.Mission
{
    [GenerateSerializer]
    public sealed class MissionDto
    {
        [Id(0)]
        public required string Name { get; set; }

        [Id(1)]
        public string? Description { get; set; }

        [Id(2)]
        public decimal Longitude { get; set; }

        [Id(3)]
        public decimal Latitude { get; set; }

        [Id(4)]
        public decimal SearchRadius { get; set; }

        [Id(5)]
        public DateTime Created { get; set; }

        [Id(6)]
        public DateTime? Updated { get; set; }

        [Id(7)]
        public string? Organization { get; set; }

        [Id(8)]
        public Guid Id { get; set; }
    }
}
