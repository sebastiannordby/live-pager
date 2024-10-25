using LivePager.Grains.Features.Participant.Repositories;
using Microsoft.Extensions.Configuration;
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
                .AddClientBuilderConfigurator<ClientConfigurator>()
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
                    .AddMemoryGrainStorage("LocationStore")
                    .AddMemoryGrainStorage("MissionStore")
                    .AddMemoryGrainStorage("PubSubStore")
                    .AddMemoryStreams("DefaultStreamProvider");
            }
        }

        private class ClientConfigurator : IClientBuilderConfigurator
        {
            public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
            {
                clientBuilder.AddMemoryStreams("DefaultStreamProvider"); // Add stream provider to client
            }
        }
    }
}
