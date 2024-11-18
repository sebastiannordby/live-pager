@description('Deploys the React frontend.')

param apiBaseUrl string

resource staticWebApp 'Microsoft.Web/staticSites@2021-03-01' = {
  name: 'livepager-frontend'
  location: resourceGroup().location
  properties: {
    branch: 'main'
    repositoryToken: '<YOUR_GITHUB_PERSONAL_ACCESS_TOKEN>'
    buildProperties: {
      appLocation: 'src'
      apiLocation: 'api'
      outputLocation: 'dist'
    }
    appSettings: [
      {
        name: 'VITE_API_URI'
        value: apiBaseUrl
      }
    ]
  }
}
