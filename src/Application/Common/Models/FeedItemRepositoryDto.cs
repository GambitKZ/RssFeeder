using RssFeeder.Domain.Interfaces;

namespace RssFeeder.Application.Common.Models;

public class FeedItemRepositoryDto : IFeedItem
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Link { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
}