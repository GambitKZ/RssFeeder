targetScope = 'resourceGroup'

@description('Location of Resources')
param location string = 'eastasia'

@description('Tags that will be set on Resource')
param tags object

var storageManagedIdentityName = 'storage-id'

// Managed Identity for Storage
resource storageManagedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: storageManagedIdentityName
  location: location
  tags: tags
}

output storageManagedIdentityPrincipalId string = storageManagedIdentity.properties.principalId
output storageManagedIdentityClientId string = storageManagedIdentity.properties.clientId
output storageManagedIdentityResourceId string = storageManagedIdentity.id