using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MVP;

public static class UploadFeed
{
    [FunctionName("UploadFeed")]
    [return: Table("rssFeed", Connection = "COSMOS_CONNECTION_STRING")]
    public static async Task<FeedItemObject> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic feedItem = JsonConvert.DeserializeObject<FeedItemRequestBody>(requestBody);

        return new FeedItemObject
        {
            PartitionKey = "MentoringProgram",
            RowKey = Guid.NewGuid().ToString(),
            Title = feedItem.Title,
            Content = feedItem.Content,
            Link = feedItem.Link
        };
    }
}