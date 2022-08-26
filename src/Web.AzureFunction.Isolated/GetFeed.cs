using System.Net;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RssFeeder.Application.RssFeed.Queries.GetRss;

namespace RssFeeder.Web.AzureFunction.Isolated;

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
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        var rssFeed = await _mediator.Send(new GetRssFeedQuery());

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/xml; charset=utf-8");

        response.WriteString(rssFeed);

        return response;
    }
}