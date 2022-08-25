# Summary

I have an E-book that don't syncronize with the PC's browser, bot works fine with the RSS.
So I will use this feature, saving necessary articles in Azure Table to read them from the E-book.

Build on "https://github.com/jasontaylordev/CleanArchitecture" Template.

Also used the "https://github.com/ardalis/CleanArchitecture/" for it's "SharedCore" idea.

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
- [ ] Add Fluent Validation
- [ ] Understand Isolated Process of the Azure Function
- [ ] Get reed of Feed Object from Shared Kernel(?)
- [ ] Create DTO for Function/Infrastructure
- [ ] Work with Behaviors
- [ ] Add Unit Test
- [ ] Add support of different RSS themes
- [ ] Add Table for keeping all available themes, and logic to work with them
- [ ] Get rid of Template's code
- [ ] Handle exception when failed to remove
- [ ] Add time of the Insertion for the order
- [ ] Handle the Duplication by using the Link as a "Row Key"
- [ ] Allow to convert HTML to pdf and provide the links to them in RSS (<https://www.syncfusion.com/blogs/post/html-to-pdf-conversion-in-csharp.aspx>)
- [ ] Feed Removal need to clear the PDF file


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