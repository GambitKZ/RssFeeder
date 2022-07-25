using MediatR;

namespace RssFeeder.Application.FeedItem.Commands.CreateFeedItem;

public class CreateFeedItemCommand : IRequest<string>
{
}

public class CreateFeedItemCommandHandler : IRequestHandler<CreateFeedItemCommand, string>
{
}