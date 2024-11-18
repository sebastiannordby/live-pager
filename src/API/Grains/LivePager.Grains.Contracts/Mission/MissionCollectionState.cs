namespace LivePager.Grains.Contracts.Mission
{
    [GenerateSerializer]
    public class MissionCollectionState
    {
        [Id(0)]
        public Dictionary<Guid, string> Missions { get; set; } = new();
    }
}
