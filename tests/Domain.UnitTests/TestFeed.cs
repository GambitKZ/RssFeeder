using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Domain.UnitTests;

internal class TestFeed : IFeedItem
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Link { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
}