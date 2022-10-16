using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using AutoMapper;
using FluentValidation;
using RssFeeder.Domain.Interfaces;
using RssFeeder.Domain.Mappings;
using RssFeeder.Domain.Validators;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Domain.Services;

public static class RssBuilderService
{
    private static DateTimeOffset? _latestDate = null;

    public static string GetRssStringFromItems(IFeedHeader feedHeader, IEnumerable<IFeedItem> listOfFeeds)
    {
        ValidateHeader(feedHeader);

        List<SyndicationItem> items = GetRssItems(listOfFeeds);
        SyndicationFeed feed = FormRssFeed(feedHeader, items);

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

    private static List<SyndicationItem> GetRssItems(IEnumerable<IFeedItem> listOfFeeds)
    {
        List<SyndicationItem> items = MapFeedItemsToRssItems(listOfFeeds);

        //listOfFeeds (item => item.Timestamp)

        foreach (var item in listOfFeeds)
        {
            if (!_latestDate.HasValue || item.Timestamp.Value > _latestDate.Value)
            {
                _latestDate = item.Timestamp.Value;
            }
        }

        // Otherwise I receive them in wrong order
        items.Reverse();
        return items;
    }

    private static List<SyndicationItem> MapFeedItemsToRssItems(IEnumerable<IFeedItem> listOfFeeds)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FeedItemToSyndicationItemMapperProfile>();
        });
        var mapper = config.CreateMapper();

        return mapper.Map<List<SyndicationItem>>(listOfFeeds);
    }

    private static void ValidateHeader(IFeedHeader feedHeader)
    {
        var validator = new RssHeaderValidator();
        validator.ValidateAndThrow(feedHeader);
    }

    private static SyndicationFeed FormRssFeed(IFeedHeader feedHeader, List<SyndicationItem> items)
    {
        SyndicationFeed feed = new SyndicationFeed(feedHeader.Title,
                    feedHeader.Description, feedHeader.AlternateLink);
        foreach (var author in feedHeader?.Authors)
        {
            feed.Authors.Add(new SyndicationPerson(author));
        }
        foreach (var category in feedHeader?.Categories)
        {
            feed.Categories.Add(new SyndicationCategory(category));
        }
        feed.Description = new TextSyndicationContent(feedHeader.Description);

        feed.Items = items;

        feed.Language = feedHeader.Language;
        feed.LastUpdatedTime = _latestDate ?? DateTime.Now;
        return feed;
    }
}