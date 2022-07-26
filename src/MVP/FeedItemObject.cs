using System;
using Azure;
using Azure.Data.Tables;

namespace MVP;

public record FeedItemObject : ITableEntity
{
    public string RowKey { get; set; } = default!;

    public string PartitionKey { get; set; } = default!;

    public string Title { get; set; }

    public string Content { get; set; }

    public string Link { get; set; }

    public ETag ETag { get; set; } = default!;

    public DateTimeOffset? Timestamp { get; set; } = default!;
}