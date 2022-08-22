using MediatR;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.FeedItem.Commands.CreateFeedItem;

public class CreateFeedItemsCommand : IRequest<string>
{
    public IList<SharedKernel.Models.FeedItem> ListOfFeeds { get; set; }
}

public class CreateFeedItemCommandHandler : IRequestHandler<CreateFeedItemsCommand, string>
{
    private readonly IRepositoryBase<IFeedItem> _repository;

    public CreateFeedItemCommandHandler(IRepositoryBase<IFeedItem> repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(CreateFeedItemsCommand request, CancellationToken cancellationToken)
    {
        // add to Context and Save
        _repository.AddRange(request.ListOfFeeds);
        await _repository.SaveChangesAsync(cancellationToken);

        // Notify that it was done
        //feed.AddDomainEvent(new FeedItemSavedEvent(feed));

        return "Feed was saved";
    }
}