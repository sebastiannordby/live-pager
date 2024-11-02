using LivePager.Grains.Contracts;
using LivePager.Grains.Contracts.Mission;
using LivePager.Grains.Contracts.Participant;
using Orleans.Streams;

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
        public async Task CreateMissionAsync_ShouldPersistToCollection()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            var missionGrain = _fixture.Cluster.GrainFactory
                .GetGrain<IMissionGrain>(grainId);
            var missionCollectionGrain = _fixture.Cluster.GrainFactory
                .GetGrain<IMissionCollectionGrain>(
                    GrainStorageConstants.GlobalMissionCollection);

            // Define test data
            var name = "Test Mission";
            var description = "Test Description";
            var longitude = 10.0m;
            var latitude = 20.0m;
            var searchRadius = 5.0m;

            // Act
            await missionGrain.CreateMissionAsync(
                name,
                description,
                longitude,
                latitude,
                searchRadius);

            // Assert
            var missions = await missionCollectionGrain.GetMissions();

            Assert.Contains(missions, x => x.Name == name);
        }

        [Fact]
        public async Task SetUserLocationAsync_ShouldStreamUserLocation()
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
            var location = new LocationDataPoint
            {
                Latitude = 1.23m,
                Longitude = 4.56m
            };
            await grain
                .SetUserLocationAsync(location);
            await TimeSpan
                .FromSeconds(2)
                .WaitUntilCollectionHasEntries(receivedLocations);

            // Assert
            Assert.Single(receivedLocations);
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
