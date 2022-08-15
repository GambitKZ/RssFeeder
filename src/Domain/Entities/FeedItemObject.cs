using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Domain.Entities;

public class FeedItemObject : BaseAuditableEntity, IFeedItem
{
    public string Title { get; set; }

    public string Content { get; set; }

    public string Link { get; set; }
    string? IFeedItem.Id { get; set; }

    public DateTimeOffset? Timestamp { get; set; }
}