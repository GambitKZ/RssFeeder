using MediatR;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.FeedItem.Commands.DeleteFeedItems;

public class DeleteFeedItemsCommand : IRequest<string>
{
    public DeleteFeedItemsCommand(IList<string> listOfFeedId)
    {
        ListOfFeedId = listOfFeedId;
    }

    public IList<string> ListOfFeedId { get; }
}

public class DeleteFeedItemsCommandHandler : IRequestHandler<DeleteFeedItemsCommand, string>
{
    private readonly IRepositoryBase<IFeedItem> _repository;

    public DeleteFeedItemsCommandHandler(IRepositoryBase<IFeedItem> repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(DeleteFeedItemsCommand request, CancellationToken cancellationToken)
    {
        _repository.DeleteRange(request.ListOfFeedId);

        await _repository.SaveChangesAsync(cancellationToken);

        return "Feeds were removed";
    }
}