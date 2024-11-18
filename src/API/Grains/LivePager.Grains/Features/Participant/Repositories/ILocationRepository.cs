using LivePager.Grains.Contracts.MissionParticipant;

namespace LivePager.Grains.Features.Participant.Repositories
{
    public interface ILocationRepository
    {
        Task SaveLocationsAsync(
            IEnumerable<LocationDataPoint> dataPoints,
            CancellationToken cancellationToken = default);
    }
}
