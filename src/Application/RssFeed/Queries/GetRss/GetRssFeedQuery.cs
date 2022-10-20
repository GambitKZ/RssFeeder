using MediatR;
using RssFeeder.Application.Common.Models;
using RssFeeder.Domain.Services;
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

        var header = new FeedHeader()
        {
            Title = "Gambit's Personal RSS",
            AlternateLink = new Uri("https://SomeURI"),
            Description = "RSS that provide the links to the articles given in mentoring program",
            Language = "en-us",
            Authors = new List<string>() { "rusnigdrag@gmail.com" },
            Categories = new List<string>() { "Mentoring URLs" }
        };

        return RssBuilderService.GetRssStringFromItems(header, listOfFeeds);
    }
}