using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using AutoMapper;
using FluentValidation;
using RssFeeder.Domain.Interfaces;
using RssFeeder.Domain.Mappings;
using RssFeeder.Domain.Validators;

namespace RssFeeder.Domain.Services;

public static class RssBuilderService
{
    private static DateTimeOffset? _latestDate = null;

    public static string GetRssStringFromItems(IFeedHeader feedHeader, IEnumerable<IFeedItem> listOfFeeds)
    {
        SyndicationFeed feed = GetSyndicationFeed(feedHeader, listOfFeeds);

        return GetXmlStringFromFeed(feed);
    }

    public static MemoryStream GetRssStreamFromItems(IFeedHeader feedHeader, IEnumerable<IFeedItem> listOfFeeds)
    {
        SyndicationFeed feed = GetSyndicationFeed(feedHeader, listOfFeeds);

        return GetFeedStream(feed);
    }

    private static SyndicationFeed GetSyndicationFeed(IFeedHeader feedHeader, IEnumerable<IFeedItem> listOfFeeds)
    {
        ValidateHeader(feedHeader);

        List<SyndicationItem> items = GetRssItems(listOfFeeds);
        GetDateOfLastUpdate(listOfFeeds);

        // TODO: Check if this step is necessary
        // Better save it correctly than revers in the end.
        items.Reverse();

        return FormRssFeed(feedHeader, items);
    }

    private static void ValidateHeader(IFeedHeader feedHeader)
    {
        RssHeaderValidator validator = new();
        validator.ValidateAndThrow(feedHeader);
    }

    private static List<SyndicationItem> GetRssItems(IEnumerable<IFeedItem> listOfFeeds)
    {
        MapperConfiguration config = new(cfg => cfg.AddProfile<FeedItemToSyndicationItemMapperProfile>());
        IMapper mapper = config.CreateMapper();

        return mapper.Map<List<SyndicationItem>>(listOfFeeds);
    }

    private static void GetDateOfLastUpdate(IEnumerable<IFeedItem> listOfFeeds)
    {
        foreach (DateTimeOffset timestamp in listOfFeeds
                                            .Where(_ => _.Timestamp.HasValue)
                                            .Select(_ => _.Timestamp!.Value))
        {
            _latestDate ??= timestamp;
            if (timestamp > _latestDate.Value)
            {
                _latestDate = timestamp;
            }
        }
    }

    private static SyndicationFeed FormRssFeed(IFeedHeader feedHeader, List<SyndicationItem> items)
    {
        SyndicationFeed feed = new(feedHeader.Title,
                    feedHeader.Description, feedHeader.AlternateLink);

        foreach (string author in feedHeader.Authors!)
        {
            feed.Authors.Add(new SyndicationPerson(author));
        }

        foreach (string category in feedHeader.Categories!)
        {
            feed.Categories.Add(new SyndicationCategory(category));
        }

        feed.Description = new TextSyndicationContent(feedHeader.Description);

        feed.Items = items;

        feed.Language = feedHeader.Language;
        feed.LastUpdatedTime = _latestDate ?? DateTime.Now;
        return feed;
    }

    private static string GetXmlStringFromFeed(SyndicationFeed feed)
    {
        XmlWriterSettings settings = new()
        {
            //Encoding = Encoding.UTF8,
            Encoding = new UTF8Encoding(false), // Most important: ensures the encoding is UTF-8 without BOM
            NewLineHandling = NewLineHandling.Entitize,
            NewLineOnAttributes = true,
            Indent = true
        };

        Rss20FeedFormatter rssFeed = new(feed, true);

        StringBuilder sb = new();
        var rssWriter = XmlWriter.Create(sb, settings);
        rssFeed.WriteTo(rssWriter);
        rssWriter.Close();

        return sb.ToString();
    }

    private static MemoryStream GetFeedStream(SyndicationFeed feed)
    {
        Rss20FeedFormatter rssFeed = new(feed);

        XmlWriterSettings settings = new()
        {
            // Most important: ensures the encoding is UTF-8 without BOM
            Encoding = new UTF8Encoding(false),
            NewLineHandling = NewLineHandling.Entitize,
            NewLineOnAttributes = true,
            Indent = true
        };

        var stream = new MemoryStream();
        using (var writer = XmlWriter.Create(stream, settings))
        {
            rssFeed.WriteTo(writer);
        }

        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }
}