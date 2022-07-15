using System.Globalization;
using RssFeeder.Application.TodoLists.Queries.ExportTodos;
using CsvHelper.Configuration;

namespace RssFeeder.Infrastructure.Files.Maps;

public class TodoItemRecordMap : ClassMap<TodoItemRecord>
{
    public TodoItemRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

        Map(m => m.Done).ConvertUsing(c => c.Done ? "Yes" : "No");
    }
}
