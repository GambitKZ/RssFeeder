namespace RssFeeder.Web.AzureFunction.Models;

public class FeedItemIncomingDto : FeedItemDto
{
    //public string Id { get; set; }

    //public DateTimeOffset UpdateTime { get; set; }
}

public abstract class FeedItemDto
{
    public string Title { get; set; }

    public string Content { get; set; }

    public string Link { get; set; }
}