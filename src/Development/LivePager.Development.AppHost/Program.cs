var builder = DistributedApplication.CreateBuilder(args);

// Configure Azurite emulator
//var azureStorage = builder
//    .AddAzureStorage("storage")
//    .RunAsEmulator();

//var azureBlob = azureStorage
//    .AddBlobs("BlobConnection");
//var azureQueue = azureStorage
//    .AddQueues("QueueConnection");

const string azuriteConnection = "UseDevelopmentStorage=true";

// Add the Silo Host project
var siloHost = builder
    .AddProject<Projects.LivePager_SiloHost>("siloHost")
    //.WithReference(azureBlob)
    //.WithReference(azureQueue)
    .WithEnvironment("Orleans:Storage:LocationStore:ContainerName", "location-store")
    .WithEnvironment("Orleans:Storage:MissionStore:ContainerName", "mission-store")
    .WithEnvironment("Orleans:Storage:PubSubStore:ContainerName", "pub-sum")
    .WithEnvironment("Orleans:Storage:BlobConnectionString", azuriteConnection)
    .WithEnvironment("Orleans:Storage:QueueConnectionString", azuriteConnection);

// Add the API Service project
var gatewayService = builder.AddProject<Projects.LivePager_Gateway>("gateway-service")
    .WithReference(siloHost)
    //.WithReference(azureBlob)
    //.WithReference(azureQueue)
    .WithEnvironment("Orleans:Storage:BlobConnectionString", azuriteConnection)
    .WithHttpsEndpoint(5170, name: "gateway-service-https"); // Ensure the API service can reference the silo host


// Add the Web Frontend project
builder.AddNpmApp("frontend", "../../Clients/livepager.frontend", "dev")
    .WithReference(gatewayService)
    .WithEnvironment("BROWSER", "none")
    .WithEnvironment("VITE_API_URI", "https://localhost:5170/") // Disable opening browser on npm start
    .WithHttpsEndpoint(5173, name: "frontend", env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
