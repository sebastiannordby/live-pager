using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using LivePager.SiloHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Orleans.Configuration;

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
                .WithImage("mcr.microsoft.com/azure-storage/azurite:3.18.0")
                .WithName("azurite_test_container")
                .WithExposedPort(10000)  // Blob Service
                .WithExposedPort(10001)  // Queue Service
                .WithPortBinding(10000, 10000)  // Map Blob service port to localhost
                .WithPortBinding(10001, 10001)  // Map Queue service port to localhost
                .WithCommand("azurite", "-l", "/data", "--blobHost", "0.0.0.0", "--queueHost", "0.0.0.0")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _azuriteContainer.StartAsync();

            // Override the Blob Storage connection string in your app settings
            //var blobStorageConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xqyb0ve7h1v9hLk3D9XbCFUySDj/dSMFfoAzurite;BlobEndpoint=http://localhost:10000/devstoreaccount1;";

            var blobStorageConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xqyb0ve7h1v9hLk3D9XbCFUySDj/dSMFfoAzurite;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";

            Environment.SetEnvironmentVariable("Orleans:Storage:ConnectionString", blobStorageConnectionString);

            _clusterClientHost = Host.CreateDefaultBuilder([])
               .UseOrleansClient((context, client) =>
               {
                   client.Configure<ClusterOptions>(options =>
                   {
                       //options.ClusterId = "my-first-cluster";
                       //options.ServiceId = "MyOrleansService";
                   })
                   .UseAzureStorageClustering(options =>
                    {
                        options.TableServiceClient = new Azure.Data.Tables.TableServiceClient(blobStorageConnectionString);
                    });
               })
               .UseConsoleLifetime()
               .Build();

            await _clusterClientHost.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _azuriteContainer.StopAsync();
            await _azuriteContainer.DisposeAsync();
            await _clusterClientHost.StopAsync();
            _clusterClientHost.Dispose();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Remove existing configurations and add custom ones for testing
                config.Sources.Clear();

                // Add appsettings configuration for tests
                config.AddJsonFile("appsettings.Test.json", optional: false);

                // Add any environment variables if needed for the test environment
                config.AddEnvironmentVariables();

                // Optionally add in-memory configuration if you want to set specific values dynamically
                // config.AddInMemoryCollection(new[]
                // {
                //     new KeyValuePair<string, string>("Orleans:Storage:ProviderName", "AzureBlob"),
                //     new KeyValuePair<string, string>("Orleans:Storage:ConnectionString",
                //         "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xqyb0ve7h1v9hLk3D9XbCFUyGSDj/dSMFfoAzurite;BlobEndpoint=http://localhost:10000/devstoreaccount1;")
                // });
            });

            builder.ConfigureServices(services =>
            {
                // Register additional test services or replace existing services here if needed
                // For example, you can replace a service with a mock
            });
        }
    }
}
