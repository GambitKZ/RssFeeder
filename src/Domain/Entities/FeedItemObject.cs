namespace RssFeeder.Domain.Entities;

public class FeedItemObject
{
    public string Title { get; set; }

    public string Content { get; set; }

    public Uri Link { get; set; }

    public string Id { get; set; }

    public DateTimeOffset UpdateTime { get; set; }
}