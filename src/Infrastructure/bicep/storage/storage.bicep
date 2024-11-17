@description('Creates a storage account for Orleans and sets up necessary containers.')
param storageAccountName string
param locationStoreName string
param missionStoreName string
param missionCollectionStoreName string
param pubSubStoreName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: storageAccountName
  location: resourceGroup().location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource storageAccountBlob 'Microsoft.Storage/storageAccounts/blobServices@2023-05-01' = {
  parent: storageAccount
  name: 'default'
}

resource locationStoreContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: storageAccountBlob
  name: locationStoreName
}

resource missionStoreContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: storageAccountBlob
  name: missionStoreName
}

resource missionCollectionStoreContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: storageAccountBlob
  name: missionCollectionStoreName
}

resource pubSubStoreContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: storageAccountBlob
  name: pubSubStoreName
}

resource queueService 'Microsoft.Storage/storageAccounts/queueServices@2022-09-01' = {
  parent: storageAccount
  name: 'default'
  properties: {} // Default properties for queue service
}

output connectionString string = listKeys(storageAccount.name, storageAccount.apiVersion).keys[0].value
