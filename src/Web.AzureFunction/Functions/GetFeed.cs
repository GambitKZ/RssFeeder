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
        //var test = await _mediator.Send(new CreateTodoListCommand());
        var rssFeed = await _mediator.Send(new GetRssFeedQuery(), cancellationToken);

        //log.LogInformation("C# HTTP trigger function processed a request.");

        //string name = req.Query["name"];

        //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //dynamic data = JsonConvert.DeserializeObject(requestBody);
        //name = name ?? data?.name;

        //string responseMessage = string.IsNullOrEmpty(name)
        //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
        //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

        // Issue of Function https://github.com/Azure/azure-functions-host/issues/2896
        return new ContentResult { Content = rssFeed, ContentType = "application/xml" };
    }
}