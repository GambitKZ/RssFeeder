# Summary

I just wanted an App that build RSS for me.

Build on "https://github.com/jasontaylordev/CleanArchitecture" Template.

Plan to use the "https://github.com/ardalis/CleanArchitecture/" for it's "SharedCore" idea.

First - implement MVP to understand what do I want.

## Plan

- [x] Write an MVP that return Hardcoded RSS
- [x] Write a logic that load the data *FROM* Azure Table
- [ ] Write a logic that allow to load the data *INTO* Azure Table

## Structure

I plan the following structure:

- MVP - a simple example on which I will do the rest of the work
- Domain - hold domain logic
- Application - hold logic, Interfaces, etc.
- Presentation.AzureFunction - API on base of the Azure Function
- Infrastructure.AzureStorage - Database (Table) on base of Azure Storage

# Description

## Domain

Business logic

[TBD]