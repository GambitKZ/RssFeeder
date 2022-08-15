# Summary

I just wanted an App that build RSS for me.

Build on "https://github.com/jasontaylordev/CleanArchitecture" Template.

Plan to use the "https://github.com/ardalis/CleanArchitecture/" for it's "SharedCore" idea.

First - implement MVP to understand what do I want.

## Plan

- [x] Write an MVP that return Hardcoded RSS
- [x] Write a logic that load the data *FROM* Azure Table
- [x] Write a logic that allow to load one feed *INTO* Azure Table
- [x] Write a logic that allow to load multiple feeds *INTO* Azure Table using Batch Insert.
- [x] Create "SharedCore" and move into it common things
- [x] Update "Application"
- [x] Implement Feed Removal in MVP
- [ ] Implement Batch Feed load

## Structure

I plan the following structure:

- MVP - a simple example on which I will do the rest of the work
- Domain - hold domain logic
- Application - hold logic, Interfaces, etc.
- Presentation.AzureFunction - API on base of the Azure Function
- Infrastructure.AzureStorage - Database (Table) on base of Azure Storage

# Description


## Domain

### Business logic




[TBD]