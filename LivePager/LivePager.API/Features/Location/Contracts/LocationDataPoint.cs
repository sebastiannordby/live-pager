﻿namespace LivePager.API.Features.Location.Contracts
{
    [GenerateSerializer]
    public class LocationDataPoint
    {
        [Id(0)]
        public decimal Longitude { get; set; }

        [Id(1)]
        public decimal Latitude { get; set; }
    }
}
