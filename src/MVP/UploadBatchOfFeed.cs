using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
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

        TableBatchOperation tableOperations = new();

        foreach (var item in feedItems)
        {
            var batchItem = new FeedItemObjectForBatch()
            {
                PartitionKey = "MentoringProgram",
                RowKey = Guid.NewGuid().ToString(),
                Title = item.Title,
                Content = item.Content,
                Link = item.Link.ToString()
            };

            tableOperations.Insert(batchItem);
        }

        // Work only with Account name and Key
        StorageCredentials storageCreds = new StorageCredentials(
            Environment.GetEnvironmentVariable("StorageAccountName"),
            Environment.GetEnvironmentVariable("StorageKey"));
        CloudStorageAccount cloudStoageAccount = new(storageCreds, useHttps: true);

        CloudTableClient cloudTableClient = cloudStoageAccount.CreateCloudTableClient();

        CloudTable cloudTable = cloudTableClient.GetTableReference("rssFeed");

        await cloudTable.CreateIfNotExistsAsync();

        await cloudTable.ExecuteBatchAsync(tableOperations);

        return new OkResult();
    }
}