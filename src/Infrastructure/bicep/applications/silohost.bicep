@description('Deploys the SiloHost Service.')

param storageAccountConnectionString string
param managedEnvironmentId string
param acrServer string
param resourceGroupLocation string
param siloHostImage string // Image for the gateway service container
param locationStoreName string
param missionStoreName string
param missionCollectionStoreName string
param pubSubStoreName string

resource siloHostService 'Microsoft.App/containerApps@2024-03-01' = {
  name: 'livepager-silohost'
  location: resourceGroupLocation
  properties: {
    managedEnvironmentId: managedEnvironmentId
    configuration: {
      ingress: {
        external: true
        targetPort: 5170 // Example target port for Gateway API
      }
      registries: [
        {
          server: acrServer
        }
      ]
      secrets: [
        {
          name: 'BlobConnectionString'
          value: storageAccountConnectionString
        }
        {
          name: 'BlobConnectionString'
          value: storageAccountConnectionString
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
    template: {
      containers: [
        {
          image: siloHostImage // Distinct image for the gateway service
          name: 'silohost-service'
          resources: {
            cpu: 1
            memory: '0.5Gi' // Minimum memory allocation
          }
          env: [
            {
              name: 'BlobConnectionString'
              secretRef: 'BlobConnectionString' // Use the secret for storage
            }
          ]
        }
      ]
    }
  }
}

output apiUri string = 'https://${siloHostService.name}.azurewebsites.net'
