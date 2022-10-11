using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Domain.Services;

public static class RssBuilderService
{
    public static string GetRssStringFromItems(IEnumerable<IFeedItem> listOfFeeds)
    {
        List<SyndicationItem> items = new();
        DateTimeOffset? latestDate = null;
        foreach (var item in listOfFeeds)
        {
            // need to map RowKey to ID
            // TODO: need to use Mapper
            items.Add(new SyndicationItem(
                item.Title,
                item.Content,
                new Uri(item.Link.Trim('\"')),
                item.Id,
                item.Timestamp.Value));

            if (!latestDate.HasValue || item.Timestamp.Value > latestDate.Value)
            {
                latestDate = item.Timestamp.Value;
            }
        }

        // Otherwise I receive them in wrong order
        items.Reverse();

        // TODO: Sounds like Business logic - move to Domain
        SyndicationFeed feed = new SyndicationFeed("Gambit's Personal RSS",
                "This is my test feed", new Uri("http://SomeURI"));
        feed.Authors.Add(new SyndicationPerson("rusnigdrag@gmail.com"));
        feed.Categories.Add(new SyndicationCategory("Mentoring URLs"));
        feed.Description = new TextSyndicationContent(
            "RSS that provide the links to the articles given in mentoring program");

        feed.Items = items;

        feed.Language = "en-us";
        feed.LastUpdatedTime = latestDate ?? DateTime.Now;

        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            NewLineHandling = NewLineHandling.Entitize,
            NewLineOnAttributes = true,
            Indent = true
        };

        Rss20FeedFormatter rssFeed = new(feed, false);

        var sb = new StringBuilder();
        XmlWriter rssWriter = XmlWriter.Create(sb, settings);
        rssFeed.WriteTo(rssWriter);
        rssWriter.Close();

        return sb.ToString();
    }
}