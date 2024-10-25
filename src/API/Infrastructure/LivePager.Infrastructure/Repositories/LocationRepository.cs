using LivePager.Grains.Contracts.Location;
using LivePager.Grains.Features.Participant.Repositories;

namespace LivePager.Infrastructure.Repositories
{
    internal class LocationRepository : ILocationRepository
    {
        public async Task SaveLocationsAsync(
           IEnumerable<LocationDataPoint> dataPoints,
           CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
