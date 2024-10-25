using LivePager.Grains.Contracts.Location;
using LivePager.Grains.Features.Participant;
using LivePager.Grains.Features.Participant.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Orleans.Configuration.Internal;
using Orleans.TestingHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiverPager.Grains.Tests.Unit
{
    public class ClusterFixture : IDisposable
    {
        public ILocationRepository LocationRepositoryMock { get; } = Substitute.For<ILocationRepository>();

        public TestCluster Cluster { get; init; }
        
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
