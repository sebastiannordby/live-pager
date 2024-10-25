namespace LivePager.Grains.Contracts.Mission
{
    [Serializable]
    public class MissionState
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<string> Participants { get; set; } = new();
    }
}
