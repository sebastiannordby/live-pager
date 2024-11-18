//using LivePager.Grains.Contracts.Mission;

//namespace LiverPager.SiloHost.Tests.Integration
//{
//    public class Tests : IClassFixture<IntegrationTestsFixture>
//    {
//        private readonly IntegrationTestsFixture _fixture;

//        public Tests(IntegrationTestsFixture fixture)
//        {
//            _fixture = fixture;
//        }

//        [Fact]
//        public async Task Test1()
//        {
//            // Arrange
//            var clusterClient = _fixture.ClusterClient.Services.GetRequiredService<IClusterClient>();
//            var grain = clusterClient.GetGrain<IMissionGrain>(Guid.NewGuid());

//            // Act
//            var result = await grain.GetMissionStateAsync();
//        }
//    }
//}