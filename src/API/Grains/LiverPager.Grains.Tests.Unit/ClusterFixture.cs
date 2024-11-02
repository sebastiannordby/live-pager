using LivePager.Grains.Contracts;
using LivePager.Grains.Features.Mission.Repositories;
using LivePager.Grains.Features.Participant.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Orleans.TestingHost;

namespace LiverPager.Grains.Tests.Unit
{
    public class ClusterFixture : IDisposable
    {
        public static ILocationRepository LocationRepositoryMock { get; } = Substitute.For<ILocationRepository>();
        public static IMissionRepository MissionRepositoryMock { get; } = Substitute.For<IMissionRepository>();

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
            Cluster.Dispose();
        }

        private class TestSiloConfigurations : ISiloConfigurator
        {
            public void Configure(
                ISiloBuilder siloBuilder)
            {
                siloBuilder
                    .AddMemoryGrainStorage(GrainStorageConstants.LocationStore)
                    .AddMemoryGrainStorage(GrainStorageConstants.MissionStore)
                    .AddMemoryGrainStorage(GrainStorageConstants.PubSubStore)
                    .AddMemoryGrainStorage(GrainStorageConstants.MissionCollectionStore)
                    .AddMemoryStreams(GrainStorageConstants.DefaultStreamProvider)
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton(LocationRepositoryMock);
                        services.AddSingleton(MissionRepositoryMock);
                    });
            }
        }

        private class ClientConfigurator : IClientBuilderConfigurator
        {
            public void Configure(
                IConfiguration configuration,
                IClientBuilder clientBuilder)
            {
                clientBuilder.AddMemoryStreams("DefaultStreamProvider"); // Add stream provider to client
            }
        }
    }
}
