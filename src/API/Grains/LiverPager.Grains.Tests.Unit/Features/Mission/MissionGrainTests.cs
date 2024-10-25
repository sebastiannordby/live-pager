using LivePager.Grains.Contracts.Mission;
using LivePager.Grains.Contracts.Participant;
using Orleans.Streams;
using System.Diagnostics;

namespace LiverPager.Grains.Tests.Unit.Features.Mission
{
    [Collection(ClusterFixtureCollection.Name)]
    public class MissionGrainTests
    {
        private readonly ClusterFixture _fixture;

        public MissionGrainTests(ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldStreamUserLocationOnUpdate()
        {
            // Arrange
            var streamProvider = _fixture.ClusterClient
                .GetStreamProvider("DefaultStreamProvider");
            var streamId = Guid.NewGuid();
            var stream = streamProvider
                .GetStream<LocationDataPoint>("LocationUpdates");

            var grain = _fixture.Cluster.GrainFactory
                .GetGrain<IMissionGrain>(Guid.NewGuid());

            // Create a mock observer for the stream to verify messages
            var receivedLocations = new List<LocationDataPoint>();
            var observer = new StreamObserver<LocationDataPoint>(receivedLocations);
            await stream.SubscribeAsync(observer);

            // Act
            var location = new LocationDataPoint { Latitude = 1.23m, Longitude = 4.56m };
            await grain.SetUserLocationAsync(location);
            // Wait until the location data is received, with a timeout
            var timeout = TimeSpan.FromSeconds(2);
            var sw = Stopwatch.StartNew();
            while (receivedLocations.Count == 0 && sw.Elapsed < timeout)
            {
                await Task.Delay(100); // Poll every 100 ms
            }

            // Assert
            Assert.Single(receivedLocations); // Check if a location was streamed
            Assert.Equal(location.Latitude, receivedLocations[0].Latitude);
            Assert.Equal(location.Longitude, receivedLocations[0].Longitude);
        }
    }

    public class StreamObserver<T> : IAsyncObserver<T>
    {
        private readonly List<T> _receivedItems;

        public StreamObserver(List<T> receivedItems)
        {
            _receivedItems = receivedItems;
        }

        public Task OnNextAsync(T item, StreamSequenceToken token = null)
        {
            _receivedItems.Add(item);
            return Task.CompletedTask;
        }

        public Task OnCompletedAsync() => Task.CompletedTask;
        public Task OnErrorAsync(Exception ex) => Task.CompletedTask;
    }
}
