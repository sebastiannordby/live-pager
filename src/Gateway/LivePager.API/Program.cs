using LivePager.API.Features.Authentication;
using LivePager.API.Features.Location;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
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
        ValidIssuer = "live-pager.com",
        ValidAudience = "live-pager.com",
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
            .AllowAnyMethod();
    });
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddOrleansClient(clientBuilder =>
    {
        clientBuilder.UseLocalhostClustering();
    });
}

var app = builder.Build();

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
app.UseLocationFeature();
app.UseAuthenticationFeature();
app.Run();
