var builder = DistributedApplication.CreateBuilder(args);

// Add the Silo Host project
var siloHost = builder.AddProject<Projects.LivePager_SiloHost>("siloHost");

// Add the API Service project
var gatewayService = builder.AddProject<Projects.LivePager_API>("gateway-service")
    .WithReference(siloHost)
    .WithHttpsEndpoint(5170, name: "gateway-service-https"); // Ensure the API service can reference the silo host

// Manually construct the API URI based on the environment configuration

// Add the Web Frontend project
builder.AddNpmApp("frontend", "../../Clients/livepager.frontend", "dev")
    .WithReference(gatewayService)
    .WithEnvironment("BROWSER", "none")
    .WithEnvironment("VITE_API_URI", "https://localhost:5170/") // Disable opening browser on npm start
    .WithHttpsEndpoint(5173, name: "frontend", env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
