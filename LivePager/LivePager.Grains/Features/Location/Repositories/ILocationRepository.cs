using LivePager.Grains.Features.Location.Contracts;

namespace LivePager.Grains.Features.Location.Repositories
{
    public interface ILocationRepository
    {
        Task SaveLocationsAsync(
            IEnumerable<LocationDataPoint> dataPoints,
            CancellationToken cancellationToken = default);
    }
}
