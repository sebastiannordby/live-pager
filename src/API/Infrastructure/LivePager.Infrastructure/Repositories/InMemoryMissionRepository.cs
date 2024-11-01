using LivePager.Grains.Features.Mission.Models;
using LivePager.Grains.Features.Mission.Repositories;

namespace LivePager.Infrastructure.Repositories
{
    public class InMemoryMissionRepository : IMissionRepository
    {
        private static List<MissionEntity> _missions = new();

        public async Task<MissionEntity[]> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(_missions.ToArray());
        }

        public async Task SaveAsync(
            MissionEntity mission,
            CancellationToken cancellationToken = default)
        {
            _missions.Add(mission);
            await Task.CompletedTask;
        }
    }
}
