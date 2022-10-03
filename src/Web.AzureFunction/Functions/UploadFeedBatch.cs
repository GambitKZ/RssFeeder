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
using RssFeeder.Application.Common.Models;
using RssFeeder.Application.FeedItem.Commands.CreateFeedItem;
using RssFeeder.Web.AzureFunction.Handlers;

namespace RssFeeder.Web.AzureFunction.Functions;

public class UploadFeedBatch
{
    private readonly IMediator _mediator;
    private readonly ExceptionHandler _exceptionHandler;

    public UploadFeedBatch(IMediator mediator, ExceptionHandler exceptionHandler)
    {
        _mediator = mediator;
        _exceptionHandler = exceptionHandler;
    }

    [FunctionName("UploadFeedBatch")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        CancellationToken cancellationToken)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        // Looks like Attributes/Middleware are possible only in "Isolated" function
        // So use Exception
        try
        {
            var feedItems = JsonConvert.DeserializeObject<List<UploadFeedItem>>(requestBody);

            await _mediator.Send(new CreateFeedItemsCommand(feedItems),
                cancellationToken);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.HandleException(ex);
        }

        return new NoContentResult();
    }
}