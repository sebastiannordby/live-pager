@description('Deploys the SiloHost Service.')
@secure()
param blobConnectionString string
@secure()
param acrUsername string
@secure()
param acrPassword string
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
    configuration: {
      ingress: {
        external: true
        targetPort: 5171
      }
      registries: [
        {
          server: acrServer
          username: acrUsername
          passwordSecretRef: 'acrpassword'
        }
      ]
      secrets: [
        {
          name: 'blobconnectionstring'
          value: blobConnectionString
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
          image: siloHostImage
          name: 'silohost-service'
          resources: {
            cpu: 1
            memory: '0.5Gi'
          }
          env: [
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

output apiUri string = 'https://${siloHostService.name}.azurewebsites.net'
