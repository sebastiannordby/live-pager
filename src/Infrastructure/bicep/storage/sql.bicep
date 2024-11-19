@description('Deploys an Azure SQL Database for Gateway Service.')
param adminUsername string
@secure()
param adminPassword string
param databaseName string
param location string
param keyVaultName string

resource livePagerKeyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' existing = {
  name: keyVaultName
}

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

resource sqlConnectionStringKeyVaultSecret 'Microsoft.KeyVault/vaults/secrets@2024-04-01-preview' = {
  parent: livePagerKeyVault
  name: 'SqlConnectionString'
  properties: {
    value: 'Server=tcp:${sqlServer.name}.${environment().sqlManagement},1433;Database=${databaseName};User ID=${adminUsername};Password=${adminPassword};Encrypt=true;Connection Timeout=30;'
  }
}

output sqlConnectionStringKeyVaultSecretName string = sqlConnectionStringKeyVaultSecret.name
