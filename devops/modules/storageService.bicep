targetScope = 'resourceGroup'

@description('Location of Resources')
param location string = 'eastasia'

@description('Name of the Storage where data will be kept')
param storageName string

@description('PrincipalId of the developer who will managed the resources')
param userPrincipalId string

@description('Managed Identity that will be using this resource')
param managedIdenityPrincipalId string

@description('Tags that will be set on Resource')
param tags object


param storageInfo object = {
 storageServiceName: storageName
 tableName: 'rssFeed'
}

var tableContributorRoleDefinitionId = '0a9a7e1f-b9d0-4cc4-a60d-0319b160aaa3'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageInfo.storageServiceName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  tags: tags
}


// Storage Admin with Table Access
resource tableAdminRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: storageAccount
  name: guid(tableContributorRoleDefinitionId, userPrincipalId, storageAccount.id)
  properties:  {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', tableContributorRoleDefinitionId)
    principalId: userPrincipalId
    principalType: 'User'
  }
}

// Managed Identity for Tables
resource tableManagedIdentityAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: storageAccount
  name: guid(tableContributorRoleDefinitionId, managedIdenityPrincipalId, storageAccount.id)
  properties:  {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', tableContributorRoleDefinitionId)
    principalId: managedIdenityPrincipalId
    principalType: 'ServicePrincipal'
  }
}

// Table
resource tableService 'Microsoft.Storage/storageAccounts/tableServices@2023-01-01' = {
  name: 'default'
  parent: storageAccount
}

resource table 'Microsoft.Storage/storageAccounts/tableServices/tables@2023-01-01' = {
  name: storageInfo.tableName
  parent: tableService
}

output storageTableUrl string = storageAccount.properties.primaryEndpoints.table
