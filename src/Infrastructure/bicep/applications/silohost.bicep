@description('Deploys the SiloHost Service.')
param storageAccountName string
param resourceGroupLocation string
param siloHostImage string 
param locationStoreName string
param missionStoreName string
param missionCollectionStoreName string
param pubSubStoreName string

resource siloHostService 'Microsoft.App/containerApps@2024-03-01' = {
  name: 'livepager-silohost'
  location: resourceGroupLocation
  properties: {
    configuration: {
      ingress: {
        external: true
        targetPort: 5171
      }
    }
    template: {
      containers: [
        {
          image: siloHostImage
          name: 'silohost-service'
          resources: {
            cpu: 1
            memory: '0.5Gi'
          }
          env: [
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
  name: guid(siloHostService.id, 'StorageBlobDataContributor')
  properties: {
    principalId: siloHostService.identity.principalId
    roleDefinitionId: 'Storage Blob Data Contributor'
    scope: resourceId('Microsoft.Storage/storageAccounts', storageAccountName)
  }
}


output apiUri string = 'https://${siloHostService.name}.azurewebsites.net'
