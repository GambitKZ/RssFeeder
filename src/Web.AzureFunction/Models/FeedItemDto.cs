namespace RssFeeder.Web.AzureFunction.Models;

public abstract class FeedItemDto
{
    public string Title { get; set; }

    public string Content { get; set; }

    public string Link { get; set; }
}