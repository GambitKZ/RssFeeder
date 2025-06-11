using System.Net;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RssFeeder.Application.RssFeed.Queries.GetRss;

namespace RssFeeder.Web.AzureFunction.Isolated.Functions;

public class GetFeed
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public GetFeed(ILoggerFactory loggerFactory, IMediator mediator)
    {
        _logger = loggerFactory.CreateLogger<GetFeed>();
        _mediator = mediator;
    }

    [Function("GetFeed")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        await using MemoryStream rssFeed = await _mediator.Send(new GetRssFeedStream());
        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/xml; charset=utf-8");

        await response.WriteBytesAsync(rssFeed.ToArray());

        return response;
    }
}