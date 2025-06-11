using MediatR;
using RssFeeder.Application.Common.Models;
using RssFeeder.Domain.Interfaces;
using RssFeeder.Domain.Services;

namespace RssFeeder.Application.RssFeed.Queries.GetRss;
public class GetRssFeedStream : IRequest<MemoryStream>
{ }

public class GetRssFeedStreamHandler : IRequestHandler<GetRssFeedStream, MemoryStream>
{
    private readonly IRepositoryBase<IFeedItem> _repository;

    public GetRssFeedStreamHandler(IRepositoryBase<IFeedItem> repository)
    {
        _repository = repository;
    }

    public async Task<MemoryStream> Handle(GetRssFeedStream request, CancellationToken cancellationToken)
    {
        IEnumerable<IFeedItem> listOfFeeds = await _repository.GetAllAsync(cancellationToken);

        var header = new FeedHeader()
        {
            Title = "Gambit's Personal RSS",
            AlternateLink = new Uri("https://SomeURI"),
            Description = "RSS that provide the links to the articles given in mentoring program",
            Language = "ru",
            Authors = ["rusnigdrag@gmail.com"],
            Categories = ["Mentoring URLs"]
        };

        return RssBuilderService.GetRssStreamFromItems(header, listOfFeeds);
    }
}