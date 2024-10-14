using LivePager.API.Features.Location.Contracts;

namespace LivePager.API.Features.Location.Repositories
{
    public interface ILocationRepository
    {
        Task SaveLocationsAsync(
            IEnumerable<LocationDataPoint> dataPoints,
            CancellationToken cancellationToken = default);
    }
}
