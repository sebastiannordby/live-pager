@echo off
color 2
set /p migname= MigrationName:
dotnet ef migrations add %migname% --project .\LivePager.Gateway.csproj --startup-project .\LivePager.Gateway.csproj
pause