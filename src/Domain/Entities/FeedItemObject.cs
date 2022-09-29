using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Domain.Entities;

// TODO - remove from the Domain!!!
public class FeedItemObject : IFeedItem
{
    public string? Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Link { get; set; }

    public DateTimeOffset? Timestamp { get; set; }
}