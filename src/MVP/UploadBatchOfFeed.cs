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

        // New instance of the TableClient class
        TableServiceClient tableServiceClient = new TableServiceClient(
            Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"));

        // New instance of TableClient class referencing the server-side table
        TableClient tableClient = tableServiceClient.GetTableClient(
            tableName: "rssFeed"
        );
        await tableClient.CreateIfNotExistsAsync();

        List<TableTransactionAction> transactions = new();

        foreach (var item in feedItems)
        {
            var entity = new FeedItemObject
            {
                PartitionKey = "MentoringProgram",
                RowKey = Guid.NewGuid().ToString(),
                Title = item.Title,
                Content = item.Content,
                Link = item.Link.ToString()
            };

            var action = new TableTransactionAction(TableTransactionActionType.Add, entity);
            transactions.Add(action);
        }

        await tableClient.SubmitTransactionAsync(transactions);

        return new OkResult();
    }
}