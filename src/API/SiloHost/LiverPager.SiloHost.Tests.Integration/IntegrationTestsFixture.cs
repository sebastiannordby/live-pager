using Azure.Storage.Blobs;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using LivePager.Grains.Contracts;
using LivePager.SiloHost;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LiverPager.SiloHost.Tests.Integration
{
    public class IntegrationTestsFixture : WebApplicationFactory<ILiverPagerSiloHostAssemblyMarker>, IAsyncLifetime
    {
        public const string CollectionName = nameof(IntegrationTestsFixture);
        private readonly IContainer _azuriteContainer;
        private IHost _clusterClientHost = null!;

        public IHost ClusterClient => _clusterClientHost;

        public IntegrationTestsFixture()
        {
            _azuriteContainer = new ContainerBuilder()
                .WithImage("mcr.microsoft.com/azure-storage/azurite")
                .WithName("azurite_test_container")
                .WithExposedPort(10000)  // Blob Service
                .WithExposedPort(10001)  // Queue Service
                .WithPortBinding(10000, 10000)  // Map Blob service port to localhost
                .WithPortBinding(10001, 10001)  // Map Queue service port to localhost
                .WithCommand("azurite", "-l", "/data", "--blobHost", "0.0.0.0", "--queueHost", "0.0.0.0")
                .Build();
        }

        async Task IAsyncLifetime.InitializeAsync()
        {
            await _azuriteContainer.StartAsync();

            var blobStorageConnectionString = "UseDevelopmentStorage=true";

            Environment.SetEnvironmentVariable("Orleans:Storage:LocationStore:ContainerName", "location-store");
            Environment.SetEnvironmentVariable("Orleans:Storage:MissionStore:ContainerName", "mission-store");
            Environment.SetEnvironmentVariable("Orleans:Storage:MissionCollectionStore:ContainerName", "mission-collection");
            Environment.SetEnvironmentVariable("Orleans:Storage:PubSubStore:ContainerName", "pub-sub");
            Environment.SetEnvironmentVariable("Orleans:Storage:BlobConnectionString", blobStorageConnectionString);
            Environment.SetEnvironmentVariable("Orleans:Storage:QueueConnectionString", blobStorageConnectionString);

            var siloHostConfigBuilder = new ConfigurationBuilder();
            siloHostConfigBuilder.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("Orleans:Storage:LocationStore:ContainerName", "location-store"),
                    new KeyValuePair<string, string>("Orleans:Storage:MissionStore:ContainerName", "mission-store"),
                    new KeyValuePair<string, string>("Orleans:Storage:MissionCollectionStore:ContainerName", "mission-collection"),
                    new KeyValuePair<string, string>("Orleans:Storage:PubSubStore:ContainerName", "pub-sub"),
                    new KeyValuePair<string, string>("Orleans:Storage:BlobConnectionString", blobStorageConnectionString),
                    new KeyValuePair<string, string>("Orleans:Storage:QueueConnectionString", blobStorageConnectionString)
                });
            var siloHostConfiguration = siloHostConfigBuilder.Build();

            var webAppFactory = this.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddConfiguration(siloHostConfiguration);
                });
            });

            // Ensure the silo is started and fully initialized
            await webAppFactory.Services.GetRequiredService<IHost>().StartAsync();

            _clusterClientHost = Host.CreateDefaultBuilder([])
               .UseOrleansClient((context, client) =>
               {
                   client
                       .ConfigureServices(services =>
                       {
                           services.AddSingleton(new BlobServiceClient(blobStorageConnectionString));
                       })
                       .UseAzureStorageClustering(options =>
                        {
                            options.TableName = GrainStorageConstants.LiverPagerClusterTable;
                            options.TableServiceClient = new Azure.Data.Tables.TableServiceClient(blobStorageConnectionString);
                        });
               })
               .UseConsoleLifetime()
               .Build();

            await _clusterClientHost.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            if (_azuriteContainer is not null)
            {
                await _azuriteContainer.StopAsync();
                await _azuriteContainer.DisposeAsync();
            }

            if (_clusterClientHost is not null)
            {
                await _clusterClientHost.StopAsync();
                _clusterClientHost.Dispose();
            }
        }
    }
}
