namespace RssFeeder.Domain.Events;

public class FeedItemSavedEvent : BaseEvent
{
    public FeedItemSavedEvent(FeedItemObject feedItemObject)
    {
        FeedItemObject = feedItemObject;
    }

    public FeedItemObject FeedItemObject { get; }
}