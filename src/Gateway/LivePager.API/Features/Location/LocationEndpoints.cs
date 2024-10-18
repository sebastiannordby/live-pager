using LivePager.API.Features.Location.Requests;
using LivePager.Grains.Contracts.Location;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LivePager.API.Features.Location
{
    public class LocationEndpoints
    {
        public static async Task<Ok> AddLocationDataPoint(
            [FromBody] AddLocationDataPointRequest request,
            [FromServices] IClusterClient clusterClient,
            [FromServices] IHubContext<LocationHub> hubContext)
        {
            var locationGrain = clusterClient
                .GetGrain<ILocationGrain>(request.UserIdentificator);

            var dataPoint = new LocationDataPoint
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            await locationGrain.AddLocationAsync(dataPoint);
            _ = hubContext.Clients.All.SendAsync("location", dataPoint);

            return TypedResults.Ok();
        }
    }
}
