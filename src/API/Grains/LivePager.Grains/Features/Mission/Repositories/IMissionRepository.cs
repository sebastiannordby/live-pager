using LivePager.Grains.Features.Mission.Models;

namespace LivePager.Grains.Features.Mission.Repositories
{
    public interface IMissionRepository
    {
        Task<MissionEntity[]> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task SaveAsync(
            MissionEntity mission,
            CancellationToken cancellationToken = default);
    }
}
