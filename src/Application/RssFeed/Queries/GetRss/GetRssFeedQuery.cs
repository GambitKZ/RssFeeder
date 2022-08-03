using System.Collections;
using MediatR;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.RssFeed.Queries.GetRss;

public class GetRssFeedQuery : IRequest<string>, IEnumerable<IFeedItem>
{
    //public string Title { get; set; }
    //public string Content { get; set; }
    //public string Link { get; set; }

    public IEnumerator<IFeedItem> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}

public class GetRssFeedQueryHandler : IRequestHandler<GetRssFeedQuery, string>
{
    public Task<string> Handle(GetRssFeedQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}