@description('Deploys the Gateway Service.')
@secure()
param sqlConnectionString string
@secure()
param storageAccountConnectionString string
@secure()
param acrUsername string
@secure()
param acrPassword string

param acrServer string
param resourceGroupLocation string
param gatewayImage string
param locationStoreName string
param missionStoreName string
param missionCollectionStoreName string
param pubSubStoreName string

resource gatewayService 'Microsoft.App/containerApps@2024-03-01' = {
  name: 'livepager-gateway'
  location: resourceGroupLocation
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    configuration: {
      ingress: {
        external: true
        targetPort: 5170
      }
      secrets: [
        {
          name: 'connectionstring'
          value: sqlConnectionString
        }
        {
          name: 'blobconnectionstring'
          value: storageAccountConnectionString
        }
        {
          name: 'acrpassword'
          value: acrPassword
        }
      ]
    }
    template: {
      containers: [
        {
          image: gatewayImage
          name: 'gateway-service'
          resources: {
            cpu: 1
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'ConnectionString'
              secretRef: 'connectionstring'
            }
            {
              name: 'BlobConnectionString'
              secretRef: 'blobconnectionstring'
            }
            {
              name: 'Orleans:Storage:LocationStore:ContainerName'
              value: locationStoreName
            }
            {
              name: 'Orleans:Storage:MissionStore:ContainerName'
              value: missionStoreName
            }
            {
              name: 'Orleans:Storage:MissionCollectionStore:ContainerName'
              value: missionCollectionStoreName
            }
            {
              name: 'Orleans:Storage:PubSubStore:ContainerName'
              value: pubSubStoreName
            }
          ]
        }
      ]
    }
  }
}

resource blobStorageRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(gatewayService.id, 'StorageBlobDataContributor')
  properties: {
    principalId: gatewayService.identity.principalId
    roleDefinitionName: 'Storage Blob Data Contributor'
    scope: resourceId('Microsoft.Storage/storageAccounts', storageAccountName)
  }
}

resource sqlDbRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(gatewayService.id, 'SQLDBContributor')
  properties: {
    principalId: gatewayService.identity.principalId
    roleDefinitionName: 'SQL DB Contributor'
    scope: resourceId('Microsoft.Sql/servers/databases', sqlServerName, sqlDatabaseName)
  }
}

output apiUri string = 'https://${gatewayService.name}.azurewebsites.net'
