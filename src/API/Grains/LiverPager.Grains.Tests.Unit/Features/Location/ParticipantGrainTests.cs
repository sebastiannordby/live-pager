using Bogus;
using LivePager.Grains.Contracts.Location;
using NSubstitute;

namespace LiverPager.Grains.Tests.Unit.Features.Location
{
    [Collection(ClusterCollection.Name)]
    public class ParticipantGrainTests
    {
        private readonly Faker _faker = new();
        private readonly ClusterFixture _fixture;

        public ParticipantGrainTests(
            ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldNotPersistLocationWhenAddingLocation()
        {
            var sut = _fixture.Cluster.GrainFactory
                .GetGrain<IParticipantGrain>(Guid.NewGuid().ToString());

            await sut.AddLocationAsync(new()
            {
                Latitude = _faker.Random.Decimal(50),
                Longitude = _faker.Random.Decimal(50)
            });

            await _fixture.LocationRepositoryMock
                .DidNotReceiveWithAnyArgs()
                .SaveLocationsAsync(default!, default);
        }
    }
}
