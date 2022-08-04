using MediatR;
using RssFeeder.Domain.Entities;
using RssFeeder.Domain.Events;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.FeedItem.Commands.CreateFeedItem;

public class CreateFeedItemCommand : IRequest<string>, IFeedItem
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Link { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
}

public class CreateFeedItemCommandHandler : IRequestHandler<CreateFeedItemCommand, string>
{
    private readonly IRepositoryBase<IFeedItem> _repository;

    public CreateFeedItemCommandHandler(IRepositoryBase<IFeedItem> repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(CreateFeedItemCommand request, CancellationToken cancellationToken)
    {
        var feed = new FeedItemObject()
        {
            Title = request.Title,
            Content = request.Content,
            Link = request.Link,
            Id = request.Id
        };

        // add to Context and Save
        _repository.Add(feed);
        await _repository.SaveChangesAsync(cancellationToken);

        // Notify that it was done
        feed.AddDomainEvent(new FeedItemSavedEvent(feed));

        return "Feed was saved";
    }
}