using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RssFeeder.Application.RssFeed.Queries.GetRss;

namespace RssFeeder.Web.AzureFunction.Functions;

public class GetFeed
{
    private readonly IMediator _mediator;

    public GetFeed(IMediator mediator)
    {
        _mediator = mediator;
    }

    [FunctionName("GetFeed")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        CancellationToken cancellationToken,
        ILogger log)
    {
        var rssFeed = await _mediator.Send(new GetRssFeedQuery(), cancellationToken);

        // Issue of Function https://github.com/Azure/azure-functions-host/issues/2896
        return new ContentResult { Content = rssFeed, ContentType = "application/xml" };
    }
}