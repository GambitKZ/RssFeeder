using System.ComponentModel.DataAnnotations.Schema;
using Azure;
using Azure.Data.Tables;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Infrastructure.AzureTable.Models;

public class FeedItemAzureTableObject : IFeedItem, ITableEntity
{
    public string RowKey { get; set; } = default!;

    public string PartitionKey { get; set; } = default!;

    public string Title { get; set; }
    public string Content { get; set; }
    public string Link { get; set; }

    public ETag ETag { get; set; } = default!;

    public DateTimeOffset? Timestamp { get; set; } = default!;

    [NotMapped]
    public string? Id { get; set; }
}