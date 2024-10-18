using LivePager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLivePagerInfrastructure();

if (builder.Environment.IsDevelopment())
{
    builder.Host.UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddMemoryGrainStorage("LocationStore");
    });
}

var app = builder.Build();

app.MapGet("/", () => "Hello from LivePager.SiloHost!");

app.Run();
