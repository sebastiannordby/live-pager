@description('Deploys the LivePager application to Azure.')
// resource acr 'Microsoft.ContainerRegistry/registries@2022-12-01' existing = {
//     name: '<acr-name>'
// }

var resourceLocation = resourceGroup().location
var acrName = 'livepager.azurecr.io'
var locationStoreName = 'location-storage'
var missionStoreName = 'mission-store'
var missionCollectionStoreName = 'mission-collection-store'
var pubSubStoreName = 'pub-sub'

resource containerAppEnv 'Microsoft.App/managedEnvironments@2024-03-01' = {
    name: 'livepager-env'
    location: resourceLocation
    properties: {
        appLogsConfiguration: {
        destination: 'log-analytics'
        }
    }
}

module storage './storage/storage.bicep' = {
  name: 'storageDeployment'
  params: {
    storageAccountName: 'livepagerstorage'
    locationStoreName: locationStoreName
    missionStoreName: missionStoreName
    missionCollectionStoreName:missionCollectionStoreName
    pubSubStoreName: pubSubStoreName
  }
}

module siloHost './applications/siloHost.bicep' = {
  name: 'siloHostDeployment'
  params: {
    storageAccountConnectionString: storage.outputs.connectionString
    acrServer: acrName
    managedEnvironmentId: containerAppEnv.id
    resourceGroupLocation: resourceLocation
    siloHostImage: '${acrName}/silohost-service:latest'
    locationStoreName: locationStoreName
    missionStoreName: missionStoreName
    missionCollectionStoreName:missionCollectionStoreName
    pubSubStoreName: pubSubStoreName
  }
}

module sql './storage/sql.bicep' = {
  name: 'sqlDeployment'
  params: {
    adminUsername: 'sqladmin'
    adminPassword: 'SecureP@ssw0rd!'
    databaseName: 'LivePagerGatewayDb'
    location: resourceLocation
  }
}

module gatewayService './applications/gateway.bicep' = {
  name: 'gatewayServiceDeployment'
  params: {
    sqlConnectionString: sql.outputs.connectionString
    storageAccountConnectionString: storage.outputs.connectionString
    managedEnvironmentId: containerAppEnv.id
    resourceGroupLocation: resourceGroup().location
    acrServer: acrName
    gatewayImage: '${acrName}/gateway-service:latest'
    locationStoreName: locationStoreName
    missionStoreName: missionStoreName
    missionCollectionStoreName:missionCollectionStoreName
    pubSubStoreName: pubSubStoreName
  }
}

// module frontend './applications/frontend.bicep' = {
//   name: 'frontendDeployment'
//   params: {
//     apiBaseUrl: gatewayService.outputs.apiUri 
//   }
// }
