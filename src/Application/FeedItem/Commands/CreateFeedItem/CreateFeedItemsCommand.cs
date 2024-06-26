﻿using AutoMapper;
using MediatR;
using RssFeeder.Application.Common.Models;
using RssFeeder.Domain.Interfaces;

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
        IEnumerable<FeedItemRepositoryDto> feedItems = GetFeedItemsFromRequest(request);

        _repository.AddRange(feedItems);

        await _repository.SaveChangesAsync(cancellationToken);

        // Notify that it was done
        //feed.AddDomainEvent(new FeedItemSavedEvent(feed));

        return "Feed was saved";
    }

    private IEnumerable<FeedItemRepositoryDto> GetFeedItemsFromRequest(CreateFeedItemsCommand request)
    {
        return _mapper.Map<IList<UploadFeedItem>,
                                            IEnumerable<FeedItemRepositoryDto>>(request.ListOfFeeds);
    }
}