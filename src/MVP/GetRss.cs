using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace MVP;

public static class GetRss
{
    [FunctionName("GetRSS")]
    [Produces("application/xml")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            NewLineHandling = NewLineHandling.Entitize,
            NewLineOnAttributes = true,
            Indent = true
        };

        Rss20FeedFormatter feed = new(await GetFeed(), false);

        // Stream Builder
        var sb = new StringBuilder();
        XmlWriter rssWriter = XmlWriter.Create(sb, settings);
        feed.WriteTo(rssWriter);
        rssWriter.Close();

        // Issue of Function https://github.com/Azure/azure-functions-host/issues/2896
        return new ContentResult { Content = sb.ToString(), ContentType = "application/xml" };
    }

    private static async Task<SyndicationFeed> GetFeed()
    {
        // New instance of the TableClient class
        TableServiceClient tableServiceClient = new(Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"));

        TableClient tableClient = tableServiceClient.GetTableClient(
            tableName: "rssFeed"
        );

        await tableClient.CreateIfNotExistsAsync();

        // Read from the Storage Table
        var feedItems = tableClient.Query<FeedItemObject>(x => x.PartitionKey == "MentoringProgram");

        // form RSS Items
        List<SyndicationItem> items = new();
        DateTimeOffset? latestDate = null;
        foreach (var item in feedItems)
        {
            items.Add(new SyndicationItem(
                item.Title,
                item.Content,
                new Uri(item.Link.Trim('\"')),
                item.RowKey,
                item.Timestamp.Value)
                );

            if (!latestDate.HasValue || item.Timestamp.Value > latestDate.Value)
            {
                latestDate = item.Timestamp.Value;
            }
        }

        // Put a html of the function
        SyndicationFeed feed = new SyndicationFeed("Gambit's Personal RSS",
                "This is my test feed", new Uri("http://SomeURI"));
        feed.Authors.Add(new SyndicationPerson("rusnigdrag@gmail.com"));
        feed.Categories.Add(new SyndicationCategory("Mentoring URLs"));
        feed.Description = new TextSyndicationContent("RSS taken from the Azure Table");

        feed.Items = items;

        feed.Language = "en-us";
        feed.LastUpdatedTime = latestDate.Value;

        return feed;
    }
}