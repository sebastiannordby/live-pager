# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/SiloHost/LivePager.SiloHost/LivePager.SiloHost.csproj", "API/SiloHost/LivePager.SiloHost/"]
COPY ["Development/LivePager.Development.ServiceDefaults/LivePager.Development.ServiceDefaults.csproj", "Development/LivePager.Development.ServiceDefaults/"]
COPY ["API/Grains/LivePager.Grains/LivePager.Grains.csproj", "API/Grains/LivePager.Grains/"]
COPY ["API/Grains/LivePager.Grains.Contracts/LivePager.Grains.Contracts.csproj", "API/Grains/LivePager.Grains.Contracts/"]
COPY ["API/Infrastructure/LivePager.Infrastructure/LivePager.Infrastructure.csproj", "API/Infrastructure/LivePager.Infrastructure/"]
RUN dotnet restore "./API/SiloHost/LivePager.SiloHost/LivePager.SiloHost.csproj"
COPY . .
WORKDIR "/src/API/SiloHost/LivePager.SiloHost"
RUN dotnet build "./LivePager.SiloHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LivePager.SiloHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LivePager.SiloHost.dll"]