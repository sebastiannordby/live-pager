using LivePager.Grains.Contracts.Participant;
using LivePager.Grains.Features.Participant.Repositories;

namespace LivePager.Infrastructure.Repositories
{
    internal class InMemoryLocationRepository : ILocationRepository
    {
        public async Task SaveLocationsAsync(
           IEnumerable<LocationDataPoint> dataPoints,
           CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
