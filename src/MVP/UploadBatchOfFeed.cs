using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace MVP;

public static class UploadBatchOfFeed
{
    [FunctionName("UploadBatchOfFeed")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic feedItems = JsonConvert.DeserializeObject<List<FeedItemRequestBody>>(requestBody);

        List<FeedItemObject> itemsToLoad = new();

        // TODO: Better to do it with Batch than insert it one by one
        TableBatchOperation tableOperations = new();

        foreach (var item in feedItems)
        {
            var itemToLoad = new FeedItemObject()
            {
                PartitionKey = "MentoringProgram",
                RowKey = Guid.NewGuid().ToString(),
                Title = item.Title,
                Content = item.Content,
                Link = item.Link
            };

            itemsToLoad.Add(itemToLoad);

            //tableOperations.Insert();
            //tableOperations.Add();
        }

        // New instance of the TableClient class
        TableServiceClient tableServiceClient = new(Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"));
        TableClient tableClient = tableServiceClient.GetTableClient(
           tableName: "rssFeed"
       );
        await tableClient.CreateIfNotExistsAsync();

        return new OkResult();
    }
}