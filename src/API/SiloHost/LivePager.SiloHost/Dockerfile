#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SiloHost/LivePager.SiloHost/LivePager.SiloHost.csproj", "SiloHost/LivePager.SiloHost/"]
COPY ["Grains/LivePager.Grains/LivePager.Grains.csproj", "Grains/LivePager.Grains/"]
COPY ["Grains/LivePager.Grains.Contracts/LivePager.Grains.Contracts.csproj", "Grains/LivePager.Grains.Contracts/"]
COPY ["Infrastructure/LivePager.Infrastructure/LivePager.Infrastructure.csproj", "Infrastructure/LivePager.Infrastructure/"]
RUN dotnet restore "./SiloHost/LivePager.SiloHost/LivePager.SiloHost.csproj"
COPY . .
WORKDIR "/src/SiloHost/LivePager.SiloHost"
RUN dotnet build "./LivePager.SiloHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LivePager.SiloHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LivePager.SiloHost.dll"]