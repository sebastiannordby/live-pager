namespace LivePager.Grains.Contracts.Mission
{
    public interface IMissionCollectionGrain : IGrain, IGrainWithStringKey
    {
        Task AddMission(Guid id, string name);
        Task<string[]> GetMissions();
    }
}
