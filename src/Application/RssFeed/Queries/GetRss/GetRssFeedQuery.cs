using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using MediatR;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.RssFeed.Queries.GetRss;

public class GetRssFeedQuery : IRequest<string>
{ }

public class GetRssFeedQueryHandler : IRequestHandler<GetRssFeedQuery, string>
{
    private readonly IRepositoryBase<IFeedItem> _repository;

    public GetRssFeedQueryHandler(IRepositoryBase<IFeedItem> repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(GetRssFeedQuery request, CancellationToken cancellationToken)
    {
        var listOfFeeds = await _repository.GetAllAsync(cancellationToken);

        List<SyndicationItem> items = new();
        DateTimeOffset? latestDate = null;
        foreach (var item in listOfFeeds)
        {
            // need to map RowKey to ID
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

        // Stream Builder
        var sb = new StringBuilder();
        XmlWriter rssWriter = XmlWriter.Create(sb, settings);
        rssFeed.WriteTo(rssWriter);
        rssWriter.Close();

        return sb.ToString();
    }
}