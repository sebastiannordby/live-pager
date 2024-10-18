var builder = DistributedApplication.CreateBuilder(args);

// Add the Silo Host project
var siloHost = builder.AddProject<Projects.LivePager_SiloHost>("siloHost");

// Add the API Service project
var gatewayService = builder.AddProject<Projects.LivePager_API>("gatewayService")
    .WithReference(siloHost); // Ensure the API service can reference the silo host

// Add the Web Frontend project
builder.AddNpmApp("react", "../../Clients/livepager.frontend")
    .WithReference(gatewayService)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
