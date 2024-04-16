targetScope = 'subscription'

@description('Location of the resource group and it\'s resources')
param location string = 'eastasia'

@description('Name of the Storage where data will be kept')
param storageName string  //'rssfeederstorage'

@description('Name of the Storage Service that will be used explicitly for the Azure Internal processes')
param storageForWebJobsName string // 'rssfeedfunctionstorage'

@description('Name of the Function with the whole logic')
param functionName string //'GambitRssFeeder'

@description('PrincipalId of the developer who will managed the resources')
param userPrincipalId string

var resourceGroupName = 'rss-feeder-rg'

param resourceTags object = {
  Project: 'RssFeed'
  Type: 'PetProject'
}

// Resource Group for all Resources
resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: resourceGroupName
  location: location
  tags: resourceTags
}

// Create Managed Identity
module managedIdentity 'modules/managedIdentity.bicep' = {
  name: 'managedIdentity'
  params: {
    location: location
    tags: resourceTags
  }
  scope: resourceGroup(rg.name)
}

// Storage Service
module storage 'modules/storageService.bicep' = {
  name: 'Storage'
  params: {
    location: location
    storageName: storageName
    userPrincipalId: userPrincipalId
    managedIdenityPrincipalId: managedIdentity.outputs.storageManagedIdentityPrincipalId
    tags: resourceTags
  }
  scope: resourceGroup(rg.name)
}

// AzureFunction
// ServiceStorage
// Log Analytics
// App Insights
module functionAndInsight 'modules/azureFunction.bicep' = {
    name: 'Function'
    params: {
        location: location
        functionName: functionName
        tags: resourceTags
        // Storage
        storageServiceUrl: storage.outputs.storageTableUrl
        storageForWebJobsName:storageForWebJobsName
        storageManagedIdentityResourceId: managedIdentity.outputs.storageManagedIdentityResourceId
        storageManagedIndentityClientId: managedIdentity.outputs.storageManagedIdentityClientId
    }
    scope: resourceGroup(rg.name)
}
