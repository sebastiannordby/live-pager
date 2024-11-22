@description('Deploys the Gateway Service.')
param sqlServerName string
param sqlDatabaseName string
param storageAccountName string
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
              value: 'Server=tcp:${sqlServerName}.${environment().sqlManagement},1433;Database=${sqlDatabaseName};Authentication=ManagedIdentity'
            }
            {
              name: 'BlobConnectionString'
              value: 'https://${storageAccountName}.blob.core.windows.net;Authentication=ManagedIdentity' 
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

resource blobStorageRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(gatewayService.id, 'StorageBlobDataContributor')
  properties: {
    principalId: gatewayService.identity.principalId
    roleDefinitionId: 'Storage Blob Data Contributor'
    scope: resourceId('Microsoft.Storage/storageAccounts', storageAccountName)
  }
}

resource sqlDbRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(gatewayService.id, 'SQLDBContributor')
  properties: {
    principalId: gatewayService.identity.principalId
    roleDefinitionId: 'SQL DB Contributor'
    scope: resourceId('Microsoft.Sql/servers/databases', sqlServerName, sqlDatabaseName)
  }
}

output apiUri string = 'https://${gatewayService.name}.azurewebsites.net'
