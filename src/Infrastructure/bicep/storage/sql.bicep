@description('Deploys an Azure SQL Database for Gateway Service.')
param adminUsername string
param adminPassword string
param databaseName string
param location string

resource sqlServer 'Microsoft.Sql/servers@2024-05-01-preview' = {
  name: '${databaseName}-server'
  location: resourceGroup().location
  properties: {
    administratorLogin: adminUsername
    administratorLoginPassword: adminPassword
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  name: databaseName
  location: location
  parent: sqlServer
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
  }
}

output connectionString string = 'Server=tcp:${sqlServer.name}.database.windows.net,1433;Database=${databaseName};User ID=${adminUsername};Password=${adminPassword};Encrypt=true;Connection Timeout=30;'
