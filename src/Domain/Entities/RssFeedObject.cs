using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Domain.Entities;

public class RssFeedObject
{
    public string FeedTable { get; set; }

    public string FeedDescription { get; set; }

    public Uri FeedUrl { get; set; }

    public string FeedAuthor { get; set; }

    public string[] FeedCategories { get; set; }

    public List<IFeedItem> Items { get; } = new();
}