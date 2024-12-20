var builder = DistributedApplication.CreateBuilder(args);

const string azuriteConnection = "UseDevelopmentStorage=true";

// Orleans needs:
// - Blob Storage
// - Queues

// ASP.Core: Add the Silo Host project
var siloHost = builder
    .AddProject<Projects.LivePager_SiloHost>("siloHost")
    .WithEnvironment("Orleans:Storage:LocationStore:ContainerName", "location-store")
    .WithEnvironment("Orleans:Storage:MissionStore:ContainerName", "mission-store")
    .WithEnvironment("Orleans:Storage:MissionCollectionStore:ContainerName", "mission-collection")
    .WithEnvironment("Orleans:Storage:PubSubStore:ContainerName", "pub-sum")
    .WithEnvironment("Orleans:Storage:BlobConnectionString", azuriteConnection)
    .WithEnvironment("Orleans:Storage:QueueConnectionString", azuriteConnection);

// ASP.Core: Add the API Service project
// Needs a MSSQL Database
var gatewayService = builder.AddProject<Projects.LivePager_Gateway>("gateway-service")
    .WithReference(siloHost)
    .WithEnvironment("Orleans:Storage:BlobConnectionString", azuriteConnection)
    .WithEnvironment("Orleans:Storage:QueueConnectionString", azuriteConnection)
    //.WithEnvironment("Secrets:ConnectionString", "Server=localhost,1433;Database=liverpager_gateway;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;MultipleActiveResultsets=True;")
    .WithHttpsEndpoint(5170, name: "gateway-service-https");

// React /w Vite: Add the Web Frontend project
builder.AddNpmApp("frontend", "../../Clients/livepager.frontend", "dev")
    .WithReference(gatewayService)
    .WithEnvironment("BROWSER", "none")
    .WithEnvironment("VITE_API_URI", "https://localhost:5170/") // Disable opening browser on npm start
    .WithHttpsEndpoint(5173, name: "frontend", env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();

// Configure Azurite emulator
//var storage = builder
//    .AddAzureStorage("storage")
//    .RunAsEmulator();

//var grainStorage = storage.AddBlobs("BlobConnection");
//var cluster = storage.AddTables("LiverPagerClusterTable");
//var queues = storage.AddQueues("QueueConnection");

//var sqlServer = builder
//    .AddSqlServer("gatewaydbserver", port: 1433);
//var sqlDatabase = sqlServer
//    .AddDatabase("gatewaydb");
