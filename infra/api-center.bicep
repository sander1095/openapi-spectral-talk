@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

@description('Tags to apply to the API Center resource')
param tags object = {
  'aspire-resource-name': 'api-center'
}

// Azure API Center resource (Free tier is the default)
resource apiCenter 'Microsoft.ApiCenter/services@2024-03-01' = {
  name: 'apic-api-linting'
  location: location
  properties: {
    // Free tier is automatically applied
  }
  tags: tags
}

// Workspace for organizing APIs (optional but recommended)
resource workspace 'Microsoft.ApiCenter/services/workspaces@2024-03-01' = {
  name: 'default'
  parent: apiCenter
  properties: {
    title: 'Default Workspace'
    description: 'Default workspace for API management'
  }
}

// Output the API Center details
output apiCenterName string = apiCenter.name
output apiCenterId string = apiCenter.id
output workspaceName string = workspace.name
