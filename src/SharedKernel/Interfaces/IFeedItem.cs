namespace RssFeeder.SharedKernel.Interfaces;

public interface IFeedItem
{
    public string? Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string Link { get; set; }

    public DateTimeOffset? Timestamp { get; set; }
}