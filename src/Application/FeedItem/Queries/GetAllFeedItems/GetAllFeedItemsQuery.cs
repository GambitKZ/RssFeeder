using MediatR;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.FeedItem.Queries.GetAllFeedItems;

public class GetAllFeedItemsQuery : IRequest<IEnumerable<IFeedItem>>
{ }

public class GetAllFeedItemsQueryHandler : IRequestHandler<GetAllFeedItemsQuery, IEnumerable<IFeedItem>>
{
    private readonly IRepositoryBase<IFeedItem> _repository;

    public GetAllFeedItemsQueryHandler(IRepositoryBase<IFeedItem> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<IFeedItem>> Handle(GetAllFeedItemsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}