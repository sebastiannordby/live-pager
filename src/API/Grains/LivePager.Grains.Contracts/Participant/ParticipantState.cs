
namespace LivePager.Grains.Contracts.Participant
{
    [Serializable]
    public class ParticipantState
    {
        [Id(0)]
        public string EmailAddress { get; set; } = null!;

        [Id(1)]
        public string DisplayName { get; set; } = null!;

        [Id(2)]
        public Guid MissionId { get; set; }

        [Id(3)]
        public List<LocationDataPoint> DataPoints { get; set; } = new List<LocationDataPoint>();
    }
}
