using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using RssFeeder.Application.FeedItem.Commands.DeleteFeedItems;

namespace RssFeeder.Web.AzureFunction;

public class DeleteFeedItems
{
    private readonly IMediator _mediator;

    public DeleteFeedItems(IMediator mediator)
    {
        _mediator = mediator;
    }

    [FunctionName("DeleteFeedItems")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
        CancellationToken cancellationToken)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic feedIds = JsonConvert.DeserializeObject<List<string>>(requestBody);

        await _mediator.Send(new DeleteFeedItemsCommand()
        {
            ListOfFeedId = feedIds
        },
        cancellationToken);

        return new NoContentResult();
    }
}