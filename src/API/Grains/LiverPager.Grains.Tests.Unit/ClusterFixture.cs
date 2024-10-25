using LivePager.Grains.Features.Participant.Repositories;
using NSubstitute;
using Orleans.TestingHost;

namespace LiverPager.Grains.Tests.Unit
{
    public class ClusterFixture : IDisposable
    {
        public ILocationRepository LocationRepositoryMock { get; } = Substitute.For<ILocationRepository>();

        public TestCluster Cluster { get; init; }
        public IClusterClient ClusterClient => Cluster.Client;

        public ClusterFixture()
        {
            Cluster = new TestClusterBuilder()
                .AddSiloBuilderConfigurator<TestSiloConfigurations>()
                .Build();

            Cluster.Deploy();
        }


        public void Dispose()
        {
            Cluster.StopAllSilos();
        }

        private class TestSiloConfigurations : ISiloConfigurator
        {
            public void Configure(ISiloBuilder siloBuilder)
            {
                siloBuilder
                    .AddMemoryGrainStorage("LocationStore");
            }
        }
    }
}
