targetScope = 'resourceGroup'

@description('Location of Resources')
param location string = 'eastasia'

@description('Name of the Function with the whole logic')
param functionName string

@description('Tags that will be set on Resource')
param tags object

// Storage
@description('PrincipalId of Managed Identity that works with Storage')
param storageManagedIdentityResourceId string

@description('Client of Managed Identity that works with Storage')
param storageManagedIndentityClientId string

@description('Url of the Storage Service')
param storageServiceUrl string

@description('Name of the Storage Service that will be used explicitly for the Azure Internal processes')
param storageForWebJobsName string

var appServicePlanName = 'EastAsiaPlan'

var logAnalyticsWorkspaceName = 'rss-feed-loganal'
var applicationInsightsName = 'rss-feeder-apins'

var functionWorkerRuntime = 'dotnet-isolated'

// Linux app wants format like \'runtime|runtimeVersion\'. For example: \'python|3.9\'')
var linuxFxVersion = '${functionWorkerRuntime}|8'

// Storage Account that keeps function's data
resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: storageForWebJobsName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'Storage'
  properties: {
    supportsHttpsTrafficOnly: true
    defaultToOAuthAuthentication: true
  }
  tags: tags
}

// Workspace for AppInsights
resource workspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: {
    retentionInDays: 90
  }
  tags: tags
}

// Application Insights
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  properties:{
    Application_Type: 'web'
    RetentionInDays: 90
    WorkspaceResourceId: workspace.id
    IngestionMode: 'LogAnalytics'
    publicNetworkAccessForIngestion: 'Enabled'
    Request_Source: 'rest'
  }
  tags: tags
}

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
  tags: tags
}

// Function 'functionapp'
resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: functionName
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${storageManagedIdentityResourceId}' : {}
    }
  }
  tags: tags
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
        // do we need linuxFxVersion?
        appSettings: [
        {
            name: 'WEBSITE_CONTENTSHARE'
            value: toLower(functionName)
        }
        {
            name: 'FUNCTIONS_EXTENSION_VERSION'
            value: '~4'
        }
        {
            name: 'FUNCTIONS_WORKER_RUNTIME'
            value: functionWorkerRuntime
        }
        {
            name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED'
            value: '1'
        }
        {
            name: 'AzureWebJobsStorage'
            value: 'DefaultEndpointsProtocol=https;AccountName=${storageForWebJobsName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
            name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
            value: 'DefaultEndpointsProtocol=https;AccountName=${storageForWebJobsName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        // App Insights
        {
            name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
            value: appInsights.properties.InstrumentationKey
        }
        // Storage
        {
          name: 'AzureStorageOptions__StorageBlobUrl'
          value: storageServiceUrl
        }
        {
          name: 'AzureStorageOptions__ManagedIdentityClientId'
          value: storageManagedIndentityClientId
        }
        {
          name: 'AzureStorageOptions__ConnectionType'
          value: 'ManagedIdentity'
        }
        ]
    }
    httpsOnly: true
  }
}