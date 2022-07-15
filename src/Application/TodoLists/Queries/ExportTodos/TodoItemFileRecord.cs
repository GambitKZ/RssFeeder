using RssFeeder.Application.Common.Mappings;
using RssFeeder.Domain.Entities;

namespace RssFeeder.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
