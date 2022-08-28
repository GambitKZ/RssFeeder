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
using RssFeeder.Application.FeedItem.Commands.CreateFeedItem;
using RssFeeder.SharedKernel.Models;
using RssFeeder.Web.AzureFunction.Handlers;

namespace RssFeeder.Web.AzureFunction;

public class UploadFeedBatch
{
    private readonly IMediator _mediator;
    private readonly ExceptionHandler _exceptionHandler;

    public UploadFeedBatch(IMediator mediator)
    {
        _mediator = mediator;
        //_exceptionHandler = exceptionHandler;
        _exceptionHandler = new ExceptionHandler();
    }

    [FunctionName("UploadFeedBatch")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        CancellationToken cancellationToken)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var feedItems = JsonConvert.DeserializeObject<List<FeedItem>>(requestBody);

        // Either make an attribute with Exception handler, or update "Catch"
        // Looks like Attributes/Middlewhere are possible only in "Isolated" function
        try
        {
            await _mediator.Send(new CreateFeedItemsCommand()
            {
                ListOfFeeds = feedItems
            },
            cancellationToken);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.HandleException(ex);
        }

        return new NoContentResult();
    }
}