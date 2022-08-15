using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RssFeeder.Application.FeedItem.Commands.CreateFeedItem;
using RssFeeder.SharedKernel.Interfaces;
using RssFeeder.SharedKernel.Models;

namespace RssFeeder.Web.AzureFunction;

public class UploadFeedBatch
{
    private readonly IMediator _mediator;

    public UploadFeedBatch(IMediator mediator)
    {
        _mediator = mediator;
    }

    [FunctionName("UploadFeedBatch")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var feedItems = JsonConvert.DeserializeObject<List<FeedItem>>(requestBody);

        // Well, it don't want to parse it.
        IList<IFeedItem> test = (IList<IFeedItem>)feedItems;

        var status = await _mediator.Send(new CreateFeedItemsCommand()
        {
            ListOfFeeds = (IList<SharedKernel.Interfaces.IFeedItem>)feedItems
        });

        var test2 = 2;

        return new OkResult();
    }
}