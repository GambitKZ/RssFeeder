using System;
using System.Collections.Generic;
using Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace MVP;

public class FeedItemObjectForBatch : ITableEntity
{
    public string RowKey { get; set; } = default!;

    public string PartitionKey { get; set; } = default!;

    public string Title { get; set; }

    public string Content { get; set; }

    public string Link { get; set; }

    public ETag ETag { get; set; } = default!;

    public DateTimeOffset? Timestamp { get; set; } = default!;

    public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
    {
        throw new NotImplementedException();
    }

    public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
    {
        throw new NotImplementedException();
    }
}