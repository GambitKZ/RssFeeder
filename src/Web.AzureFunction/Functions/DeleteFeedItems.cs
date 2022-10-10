using System;
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
using RssFeeder.Web.AzureFunction.Handlers;

namespace RssFeeder.Web.AzureFunction.Functions;

public class DeleteFeedItems
{
    private readonly IMediator _mediator;
    private readonly ExceptionHandler _exceptionHandler;

    public DeleteFeedItems(IMediator mediator, ExceptionHandler exceptionHandler)
    {
        _mediator = mediator;
        _exceptionHandler = exceptionHandler;
    }

    [FunctionName("DeleteFeedItems")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
        CancellationToken cancellationToken)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        try
        {
            dynamic feedIds = JsonConvert.DeserializeObject<List<string>>(requestBody);

            await _mediator.Send(new DeleteFeedItemsCommand(feedIds),
                                                cancellationToken);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.HandleException(ex);
        }

        return new NoContentResult();
    }
}