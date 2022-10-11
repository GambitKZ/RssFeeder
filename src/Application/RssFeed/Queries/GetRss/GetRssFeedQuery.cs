using MediatR;
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

        return RssBuilderService.GetRssStringFromItems(listOfFeeds);
    }
}