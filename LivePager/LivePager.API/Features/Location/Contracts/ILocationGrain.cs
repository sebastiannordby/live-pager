﻿namespace LivePager.API.Features.Location.Contracts
{
    public interface ILocationGrain : IGrainWithStringKey
    {
        Task AddLocationAsync(
            LocationDataPoint dataPoint);
        Task<LocationDataPoint[]> GetDataPointsAsync();
    }
}
