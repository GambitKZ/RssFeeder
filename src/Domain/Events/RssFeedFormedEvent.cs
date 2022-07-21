namespace RssFeeder.Domain.Events;

public class RssFeedFormedEvent : BaseEvent
{
    public RssFeedFormedEvent(RssFeedObject rssFeedObject)
    {
        RssFeedObject = rssFeedObject;
    }

    public RssFeedObject RssFeedObject { get; }
}