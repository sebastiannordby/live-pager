# LivePager

LivePager is a scalable real-time tracking application built with Orleans .NET. It is designed to assist in search-and-rescue missions, helping teams track their progress in locating missing people, pets, or other items.

The app enables users to create "missions" where a search area is defined by a radius from a central point. Members of the team can join the mission and update their location in real-time, allowing everyone involved to see where they are and coordinate their efforts effectively.

Key features include:

Mission Creation: Define search areas and invite team members to join.
Real-Time Location Tracking: Track the location of all users participating in the mission on a map.
Search Coordination: Collaborate with others by visually seeing areas that have been covered and which team members are currently searching.
Mission Overview: A centralized view of the mission's progress, showing all active participants, search areas, and coverage.
LivePager is ideal for use in search-and-rescue operations, organized search missions for missing persons, pets, or other situations that require team-based location tracking.

## How do i run this project?

### Azurite

This application is made to run on Azure and to avoid local development directly against Azure you have to use Azurite.
https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio%2Cblob-storage

Pull the Azurite-image:

```
docker pull mcr.microsoft.com/azure-storage/azurite
```

Run Azurite:

```
docker run --name liverpager-azurite -p 10000:10000 -p 10001:10001 -p 10002:10002 \
    mcr.microsoft.com/azure-storage/azurite
```

### Development

To avoid having to start all services manually i applied the use of .NET Aspire, this acts as a container orchestration tool, just like Docker Compose.

You can find the guide for how to install .NET Aspire here: https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=windows&pivots=visual-studio

When opening the solution in Visual Studio:

- Navigate to the Development folder and then LivePager.Development.AppHost, set this as your startup project
- Make sure Azurite is running
- Start
- Aspire Dashboard will show up with an overview of the resources/services/applications available

```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=liverPagerPassword" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```
