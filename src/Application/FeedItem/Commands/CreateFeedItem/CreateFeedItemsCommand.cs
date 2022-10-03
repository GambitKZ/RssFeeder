using AutoMapper;
using MediatR;
using RssFeeder.Application.Common.Models;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.FeedItem.Commands.CreateFeedItem;

public class CreateFeedItemsCommand : IRequest<string>
{
    public CreateFeedItemsCommand(IList<UploadFeedItem> listOfFeeds)
    {
        ListOfFeeds = listOfFeeds;
    }

    public IList<UploadFeedItem> ListOfFeeds { get; }
}

public class CreateFeedItemCommandHandler : IRequestHandler<CreateFeedItemsCommand, string>
{
    private readonly IRepositoryBase<IFeedItem> _repository;
    private readonly IMapper _mapper;

    public CreateFeedItemCommandHandler(IRepositoryBase<IFeedItem> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateFeedItemsCommand request, CancellationToken cancellationToken)
    {
        var feedItems = _mapper.Map<IList<UploadFeedItem>,
                                    IEnumerable<FeedItemRepositoryDto>>(request.ListOfFeeds);

        _repository.AddRange(feedItems);

        await _repository.SaveChangesAsync(cancellationToken);

        // Notify that it was done
        //feed.AddDomainEvent(new FeedItemSavedEvent(feed));

        return "Feed was saved";
    }
}