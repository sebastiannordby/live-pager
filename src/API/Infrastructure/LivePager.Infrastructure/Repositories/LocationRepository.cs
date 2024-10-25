using LivePager.Grains.Contracts.Participant;
using LivePager.Grains.Features.Location.Repositories;

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
