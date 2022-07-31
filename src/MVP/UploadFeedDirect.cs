using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MVP;

public static class UploadFeedDirect
{
    [FunctionName("UploadFeedDirect")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic feedItem = JsonConvert.DeserializeObject<FeedItemRequestBody>(requestBody);

        // New instance of the TableClient class
        TableServiceClient tableServiceClient = new TableServiceClient(
            Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"));

        // New instance of TableClient class referencing the server-side table
        TableClient tableClient = tableServiceClient.GetTableClient(
            tableName: "rssFeed"
        );
        await tableClient.CreateIfNotExistsAsync();

        var item = new FeedItemObject
        {
            PartitionKey = "MentoringProgram",
            RowKey = Guid.NewGuid().ToString(),
            Title = feedItem.Title,
            Content = feedItem.Content,
            Link = feedItem.Link.ToString()
        };

        await tableClient.AddEntityAsync(item);

        return new OkResult();
    }
}