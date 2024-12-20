using Azure.Storage.Blobs;
using LivePager.Gateway.Features.Authentication;
using LivePager.Gateway.Features.Mission;
using LivePager.Gateway.Infrastructure;
using LivePager.Grains.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.AddLiverPagerServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddInfrastructure(builder.Configuration);

builder.AddMissionFeature();
builder.Services.AddAuthenticationFeature();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "livepager.no",
        ValidAudience = "livepager.no",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Keys:Authentication:Secret"]!))
    };
});

builder.Services.AddAuthorization(options =>
{
    // Set a default policy that requires authentication
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(x =>
{
    var clients = builder.Configuration
        .GetSection("Keys:Cors:Clients")
        .Get<string[]>()!;

    x.AddPolicy("frontend", configurePolicy =>
    {
        configurePolicy
            .WithOrigins(clients)
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod();
    });
});

builder.Services.AddOrleansClient(clientBuilder =>
{
    var blobStorageConnectionString = builder.Configuration["Orleans:Storage:BlobConnectionString"];
    var queueConnectionString = builder.Configuration["Orleans:Storage:QueueConnectionString"];

    clientBuilder.ConfigureServices(services =>
    {
        services.AddSingleton(new BlobServiceClient(blobStorageConnectionString));
        services.AddHostedService<MissionStreamHostedService>();
    });


    clientBuilder.AddAzureQueueStreams(LivePagerOrleansConstants.DefaultStreamProvider,
        optionsBuilder => optionsBuilder.Configure(
            options => options.QueueServiceClient = new(queueConnectionString)));

    clientBuilder.UseAzureStorageClustering(options =>
    {
        options.TableName = LivePagerOrleansConstants.LiverPagerClusterTable;
        options.TableServiceClient = new(blobStorageConnectionString);
    });

});

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider
        .GetRequiredService<LiverPagerDbContext>();

    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("frontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMissionEndpoints();
app.UseAuthenticationFeature();
app.Run();
