var builder = DistributedApplication.CreateBuilder(args);

// Add the Silo Host project
var siloHost = builder.AddProject<Projects.LivePager_SiloHost>("siloHost");

// Add the API Service project
var apiService = builder.AddProject<Projects.LivePager_API>("apiService")
    .WithReference(siloHost); // Ensure the API service can reference the silo host

// Add the Web Frontend project
builder.AddProject<Projects.livepager_frontend>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
