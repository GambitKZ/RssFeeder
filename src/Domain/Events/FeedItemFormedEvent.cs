namespace RssFeeder.Domain.Events;

public class FeedItemFormedEvent : BaseEvent
{
    public FeedItemFormedEvent(FeedItemObject feedItemObject)
    {
        FeedItemObject = feedItemObject;
    }

    public FeedItemObject FeedItemObject { get; }
}