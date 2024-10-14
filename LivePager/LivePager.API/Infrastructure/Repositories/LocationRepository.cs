using LivePager.API.Features.Location.Contracts;
using LivePager.API.Features.Location.Repositories;

namespace LivePager.API.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        public async Task SaveLocationsAsync(
            IEnumerable<LocationDataPoint> dataPoints,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
