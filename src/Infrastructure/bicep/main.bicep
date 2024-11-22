@description('Deploys the LivePager application to Azure.')
@secure()
param sqlDatabasePassword string

@secure()
param appObjectId string

@secure()
param acrUsername string

@secure()
param acrPassword string

var resourceLocation = resourceGroup().location
var acrName = 'livepager.azurecr.io'
var locationStoreName = 'location-storage'
var missionStoreName = 'mission-store'
var missionCollectionStoreName = 'mission-collection-store'
var pubSubStoreName = 'pub-sub'

resource livePagerKeyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' = {
  name: 'livePagerKeyVault'
  location: resourceLocation
  properties: {
    enabledForTemplateDeployment: true
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: appObjectId
        permissions: {
          secrets: ['get', 'list', 'set']
        }
      }
    ]
  }
}

module storage './storage/storage.bicep' = {
  name: 'storageDeployment'
  params: {
    storageAccountName: 'livepagerstorage'
    locationStoreName: locationStoreName
    missionStoreName: missionStoreName
    missionCollectionStoreName: missionCollectionStoreName
    pubSubStoreName: pubSubStoreName
    keyVaultName: livePagerKeyVault.name
  }
}

module sql './storage/sql.bicep' = {
  name: 'sqlDeployment'
  params: {
    adminUsername: 'sqladmin'
    adminPassword: sqlDatabasePassword
    databaseName: 'LivePagerGatewayDb'
    location: resourceLocation
    keyVaultName: livePagerKeyVault.name
  }
}

module siloHost './applications/silohost.bicep' = {
  name: 'siloHostDeployment'
  params: {
    storageAccountName: 'livepagerstorage'
    resourceGroupLocation: resourceLocation
    siloHostImage: '${acrName}/silohost-service:latest'
    locationStoreName: locationStoreName
    missionStoreName: missionStoreName
    missionCollectionStoreName: missionCollectionStoreName
    pubSubStoreName: pubSubStoreName
  }
}

module gatewayService './applications/gateway.bicep' = {
  name: 'gatewayServiceDeployment'
  params: {
    storageAccountName: 'livepagerstorage'
    resourceGroupLocation: resourceGroup().location
    gatewayImage: '${acrName}/gateway-service:latest'
    locationStoreName: locationStoreName
    missionStoreName: missionStoreName
    missionCollectionStoreName: missionCollectionStoreName
    pubSubStoreName: pubSubStoreName
    sqlDatabaseName: 'LivePagerGatewayDb'
    sqlServerName: 'LivePagerGatewayDb-server'
  }
}

// module frontend './applications/frontend.bicep' = {
//   name: 'frontendDeployment'
//   params: {
//     apiBaseUrl: gatewayService.outputs.apiUri 
//   }
// }
