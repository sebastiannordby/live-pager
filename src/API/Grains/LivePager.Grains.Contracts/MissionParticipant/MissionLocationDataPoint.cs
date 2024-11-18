namespace LivePager.Grains.Contracts.MissionParticipant
{
    [GenerateSerializer]
    public class MissionLocationDataPoint
    {
        [Id(0)]
        public Guid MissionId { get; set; }

        [Id(1)]
        public Guid ParticipantId { get; set; }

        [Id(2)]
        public Guid ParticipantDisplayName { get; set; }

        [Id(3)]
        public decimal Longitude { get; set; }

        [Id(4)]
        public decimal Latitude { get; set; }
    }
}
