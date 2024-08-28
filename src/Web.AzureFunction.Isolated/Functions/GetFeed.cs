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
        string rssFeed = await _mediator.Send(new GetRssFeedQuery());
        //MemoryStream rssFeed = await _mediator.Send(new GetRssFeedQuery());

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/xml; charset=utf-8");

        //response.WriteString(rssFeed);

        // Define the correct XML declaration
        string xmlDeclaration = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

        // Check if the rssFeed starts with an XML declaration and remove it
        if (rssFeed.StartsWith("<?xml"))
        {
            int endOfDeclaration = rssFeed.IndexOf("?>");
            if (endOfDeclaration != -1)
            {
                rssFeed = rssFeed.Substring(endOfDeclaration + 2);
            }
        }

        // Trim any whitespace and then prepend with the correct declaration
        rssFeed = xmlDeclaration + Environment.NewLine + rssFeed.TrimStart();

        // Now write the modified string to your response
        response.WriteString(rssFeed);
        //await response.WriteAsBinaryAsync(rssFeed.ToArray());
        //rssFeed.Close();

        return response;


    }
}