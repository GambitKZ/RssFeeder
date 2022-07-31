using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.SharedKernel.Models;

public class FeedItem : IFeedItem
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Link { get; set; }
}