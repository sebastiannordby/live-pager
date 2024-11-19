@description('Deploys the Gateway Service.')
@secure()
param sqlConnectionString string
@secure()
param storageAccountConnectionString string
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
  properties: {
    configuration: {
      ingress: {
        external: true
        targetPort: 5170
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
          image: gatewayImage
          name: 'gateway-service'
          resources: {
            cpu: 1
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'ConnectionString'
              secretRef: 'ConnectionString'
            }
            {
              name: 'BlobConnectionString'
              secretRef: 'BlobConnectionString'
            }
          ]
        }
      ]
    }
  }
}

output apiUri string = 'https://${gatewayService.name}.azurewebsites.net'
