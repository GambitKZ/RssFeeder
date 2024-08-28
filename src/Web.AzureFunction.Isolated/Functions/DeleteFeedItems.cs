using System.Net;
using System.Text.Json;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using RssFeeder.Application.FeedItem.Commands.DeleteFeedItems;

namespace RssFeeder.Web.AzureFunction.Isolated.Functions;

public class DeleteFeedItems
{
    private readonly IMediator _mediator;

    public DeleteFeedItems(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Function("DeleteFeedItems")]
    public async Task<HttpResponseData> RunAsync(
      [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequestData req,
      CancellationToken cancellationToken)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        dynamic feedIds = JsonSerializer.Deserialize<List<string>>(requestBody);

        await _mediator.Send(new DeleteFeedItemsCommand(feedIds),
                                            cancellationToken);
        var response = req.CreateResponse(HttpStatusCode.OK);
        return response;
    }
}