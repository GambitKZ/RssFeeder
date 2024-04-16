[![Build](https://github.com/GambitKZ/RssFeeder/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/GambitKZ/RssFeeder/actions/workflows/build.yml)

# Summary

I have an E-book that don't synchronize with the PC's browser, but works fine with the RSS.
So I will use this feature, saving necessary articles in Azure Table to read them from the E-book.

Build on [following Template](https://github.com/jasontaylordev/CleanArchitecture).

## Plan

- [x] Write an MVP that return Hardcoded RSS
- [x] Write a logic that load the data *FROM* Azure Table
- [x] Write a logic that allow to load one feed *INTO* Azure Table
- [x] Write a logic that allow to load multiple feeds *INTO* Azure Table using Batch Insert.
- [x] Create "SharedCore" and move into it common things
- [x] Update "Application"
- [x] Implement Feed Removal in MVP
- [x] Implement Batch Feed load
- [x] Implement Batch Feed removal
- [x] Remove MVP and Deploy to Azure
- [x] Add Fluent Validation
- [x] Understand Isolated Process of the Azure Function
- [x] Get rid of Feed Object from Shared Kernel(?)
- [x] Add Unit Test
- [x] Create DTO for Function/Infrastructure
- [x] Get rid of Template's code
- [x] Remove "Shared Kernel" as there is no need for such a small project
- [ ] Use CI/CD tool (Bicep) to deploy the RSS to Azure
- [ ] Work with Behaviors
- [ ] Add support of different RSS themes (Mentoring/Info)
- [ ] Add Table for keeping all available themes, and logic to work with them (List of themes)
- [ ] Handle exception when failed to remove
- [ ] Keep and Provide the RSS Headers from *Infrastructure*
- [ ] Add time of the Insertion for the order
- [ ] Handle the Duplication by using the Link as a "Row Key"
- [ ] Allow to convert HTML to pdf and provide the links to them in RSS. As an [option](https://www.syncfusion.com/blogs/post/html-to-pdf-conversion-in-csharp.aspx)
- [ ] Feed Removal need to clear the PDF file
- [ ] Configure the Pre-Commit checks from [here](https://gsferreira.com/archive/2022/embedding-dotnet-format-in-your-development-cycle/)
- [ ] Return 'Problem Details' like [here](https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses)
- [ ] Use container to test Dependency. In this case Azure Table. Use ['TestContainers'](https://testcontainers.com/) 


## Structure

I plan the following structure:

- ~~MVP - a simple example on which I will do the rest of the work~~
- ~~SharedKernel - hold common things that is shared by ALL layers. **Holds interfaces for Repository, and FeedItem**~~
- Domain - hold domain logic, models, etc. **Should be used only by Application.**  
*Is it necessary though? It can be moved to Application and made private.*
- Application - hold logic, Interfaces, etc. Used in all upper proj. **
- Infrastructure.AzureStorage - Database (Table) on base of Azure Storage.
- Web.AzureFunction - API on base of the Azure Function. Presentation App.
- Web.AzureFunction.Isolated - same as above, but using the [Isolated process](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide). Presentation App.

## Description

### Domain logic

Works with **SyndicationFeed** internally to build the desired RSS that returned as XML string.

RSS Headers and Items are provided by *Application*.

*Infrastructure* works as persistence Storage.

Headers are hardcoded now, but should be also provided by *Infrastructure*.

[TBD]

### Git Hooks

Has Git Hooks that test the App before submitting the code.

## CI/CD

### Application

Use `Git Actions` to build the app and deploy it to Azure Function

### Infrastructure

Use 'Bicep" to setup the necessary infrastructure:
- Storage
- Function
- Managed Identity
- Log Analytics with App Insights

Run automatically by CI/CD, but for manual run use the following command
```
az deployment sub create --location northeurope --template-file main.bicep --parameters prodParameters.json
```