using Azure.Storage.Blobs;
using LivePager.Grains.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var orleansSettings = builder.Configuration.GetSection("Orleans");
var blobStorageConnectionString = orleansSettings["Storage:BlobConnectionString"];
var queueConnectionString = orleansSettings["Storage:QueueConnectionString"];

builder.AddLiverPagerServiceDefaults();
builder.UseOrleans(siloBuilder =>
{
    siloBuilder.UseAzureStorageClustering(opt =>
    {
        opt.TableName = LivePagerOrleansConstants.LiverPagerClusterTable;
        opt.TableServiceClient = new(blobStorageConnectionString);
    });

    siloBuilder
        .ConfigureServices(services =>
        {
            services.AddSingleton(new BlobServiceClient(blobStorageConnectionString));
            services.AddAzureBlobGrainStorageAsDefault(opt =>
            {
                opt.BlobServiceClient = new BlobServiceClient(blobStorageConnectionString);
            });
        })
        .AddAzureBlobGrainStorage(LivePagerOrleansConstants.LocationStore, options =>
        {
            options.ContainerName = orleansSettings["Storage:LocationStore:ContainerName"];
            options.BlobServiceClient = new(blobStorageConnectionString);
        })
        .AddAzureBlobGrainStorage(LivePagerOrleansConstants.MissionStore, options =>
        {
            options.ContainerName = orleansSettings["Storage:MissionStore:ContainerName"];
            options.BlobServiceClient = new(blobStorageConnectionString);
        })
        .AddAzureBlobGrainStorage(LivePagerOrleansConstants.MissionCollectionStore, options =>
        {
            options.ContainerName = orleansSettings["Storage:MissionCollectionStore:ContainerName"];
            options.BlobServiceClient = new(blobStorageConnectionString);
        })
        .AddAzureBlobGrainStorage(LivePagerOrleansConstants.PubSubStore, options =>
        {
            options.ContainerName = orleansSettings["Storage:PubSubStore:ContainerName"];
            options.BlobServiceClient = new(blobStorageConnectionString);
        }).AddAzureQueueStreams(LivePagerOrleansConstants.DefaultStreamProvider, configurator =>
        {
            configurator.ConfigureAzureQueue(
                ob => ob.Configure(options =>
                {
                    options.QueueServiceClient = new(queueConnectionString);
                    options.QueueNames = new List<string>
                    {
                        "yourprefix-azurequeueprovider-0"
                    };
                }));
            configurator.ConfigureCacheSize(1024);
            configurator.ConfigurePullingAgent(ob => ob.Configure(options =>
            {
                options.GetQueueMsgsTimerPeriod = TimeSpan.FromMilliseconds(200);
            }));
        });
});

var app = builder.Build();

app.MapGet("/", () => "Hello from LivePager.SiloHost!");

app.Run();

public partial class Program { }