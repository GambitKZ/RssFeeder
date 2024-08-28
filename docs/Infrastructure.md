# Summary

Infrastructure created via Bicep files.
Infrastructure code lay under the "devops" folder.
All DevOps activities needs to be done under branches with "devops/*" prefix.

## Bicep

More about Bicep can be read [here](https://learn.microsoft.com/en-us/training/paths/fundamentals-bicep/)

Currently I have the following modules:

- `managedIdentity` - Responsible for credential-less interaction with services
- `storageService` - Create Storage that keeps the data for RSS logic
- `azureFunction` - have several things under the hood:
  - Log Analytics and App Insights
  - Storage for Azure Function needs
  - App Service Plan and Azure Function

Azure Function is Isolated and run on Linux.

To do that via `Git Action` specific actions needs to be done.

## Deployment

### Manually

Manual run requires:

- `location` where it keep the incoming bicep request(?). Anyway, for me it `northeurope`. It doesn't affect the location of resources.
- `template-file` - file with bicep entry point. Can be an Url.
- `parameters` - file with parameters. As I want some parameters to be secret, you need to provide them manually. 

 providing several parameters that 
```
az deployment sub create --location northeurope --template-file main.bicep --parameters prodParameters.json
```

### GitHub Actions

#### Azure side pre-requisites

#### Action