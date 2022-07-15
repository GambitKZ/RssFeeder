﻿using System.Globalization;
using RssFeeder.Application.Common.Interfaces;
using RssFeeder.Application.TodoLists.Queries.ExportTodos;
using RssFeeder.Infrastructure.Files.Maps;
using CsvHelper;

namespace RssFeeder.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
