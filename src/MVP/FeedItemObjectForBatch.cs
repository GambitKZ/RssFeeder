using Microsoft.WindowsAzure.Storage.Table;

namespace MVP;

public class FeedItemObjectForBatch : TableEntity
{
    public string Title { get; set; }

    public string Content { get; set; }

    public string Link { get; set; }
}