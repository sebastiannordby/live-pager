using LivePager.Grains.Contracts.Location;

namespace LivePager.Grains.Features.Location.Repositories
{
    public interface ILocationRepository
    {
        Task SaveLocationsAsync(
            IEnumerable<LocationDataPoint> dataPoints,
            CancellationToken cancellationToken = default);
    }
}
