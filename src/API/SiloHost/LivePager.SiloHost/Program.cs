using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

builder.AddLiverPagerServiceDefaults();
builder.UseOrleans(siloBuilder =>
{
    var orleansSettings = builder.Configuration.GetSection("Orleans");
    var blobStorageConnectionString = orleansSettings["Storage:BlobConnectionString"];
    var queueConnectionString = orleansSettings["Storage:QueueConnectionString"];

    siloBuilder.UseAzureStorageClustering(opt =>
    {
        opt.TableName = "LiverPagerClusterTable";
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
        .AddAzureBlobGrainStorage("LocationStore", options =>
        {
            options.ContainerName = orleansSettings["Storage:LocationStore:ContainerName"];
            options.BlobServiceClient = new(blobStorageConnectionString);
        })
        .AddAzureBlobGrainStorage("MissionStore", options =>
        {
            options.ContainerName = orleansSettings["Storage:MissionStore:ContainerName"];
            options.BlobServiceClient = new(blobStorageConnectionString);
        })
        .AddAzureBlobGrainStorage("MissionCollectionStore", options =>
        {
            options.ContainerName = orleansSettings["Storage:MissionCollectionStore:ContainerName"];
            options.BlobServiceClient = new(blobStorageConnectionString);
        })
        .AddAzureBlobGrainStorage("PubSubStore", options =>
        {
            options.ContainerName = orleansSettings["Storage:PubSubStore:ContainerName"];
            options.BlobServiceClient = new(blobStorageConnectionString);
        }).AddAzureQueueStreams("DefaultStreamProvider", configurator =>
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