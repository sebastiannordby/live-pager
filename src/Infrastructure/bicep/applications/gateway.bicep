@description('Deploys the Gateway Service.')

param sqlConnectionString string
param storageAccountConnectionString string
param managedEnvironmentId string
param acrServer string
param resourceGroupLocation string
param gatewayImage string // Image for the gateway service container
param locationStoreName string
param missionStoreName string
param missionCollectionStoreName string
param pubSubStoreName string

resource gatewayService 'Microsoft.App/containerApps@2024-03-01' = {
  name: 'livepager-gateway'
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
          name: 'ConnectionString'
          value: sqlConnectionString
        }
        {
          name: 'BlobConnectionString'
          value: storageAccountConnectionString
        }
      ]
    }
    template: {
      containers: [
        {
          image: gatewayImage // Distinct image for the gateway service
          name: 'gateway-service'
          resources: {
            cpu: 1
            memory: '0.5Gi' // Minimum memory allocation
          }
          env: [
            {
              name: 'ConnectionString'
              secretRef: 'ConnectionString' // Use the secret for SQL connection string
            }
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

output apiUri string = 'https://${gatewayService.name}.azurewebsites.net'
