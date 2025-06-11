using System.Net;
using System.Text.Json;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using RssFeeder.Application.Common.Models;
using RssFeeder.Application.FeedItem.Commands.CreateFeedItem;

namespace RssFeeder.Web.AzureFunction.Isolated.Functions;

public class UploadFeedBatch
{
    private readonly IMediator _mediator;

    public UploadFeedBatch(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Function("UploadFeedBatch")]
    public async Task<HttpResponseData> RunAsync(
       [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req,
       CancellationToken cancellationToken)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        List<UploadFeedItem>? feedItems = JsonSerializer.Deserialize<List<UploadFeedItem>>(requestBody);

        await _mediator.Send(new CreateFeedItemsCommand(feedItems!),
            cancellationToken);

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        return response;
    }
}