using AutoMapper;
using Azure.Data.Tables;
using RssFeeder.Infrastructure.AzureTable.Models;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Infrastructure.AzureTable;

public class AzureTableRepository<T> : IRepositoryBase<T> where T : class, IFeedItem
{
    // TODO: Move from constant to Property
    private const string MentoringPartitionKey = "MentoringProgram";

    private readonly IMapper _mapper;
    private List<TableTransactionAction> _transactionLog = new();

    public AzureTableRepository(string connectionString, string tableName, IMapper mapper)
    {
        var tableServiceClient = new TableServiceClient(connectionString);

        // TODO: Pass the Table name not in Constructor?
        TableClient = tableServiceClient.GetTableClient(tableName: tableName);
        _mapper = mapper;
    }

    private TableClient TableClient { get; }

    public void Add(T entity)
    {
        var feedObject = new FeedItemAzureTableObject
        {
            PartitionKey = MentoringPartitionKey,
            RowKey = Guid.NewGuid().ToString(),
            Title = entity.Title,
            Content = entity.Content,
            Link = entity.Link
        };

        AddTransaction(feedObject, TableTransactionActionType.Add);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        foreach (var item in entities)
        {
            var feedObject = new FeedItemAzureTableObject
            {
                PartitionKey = MentoringPartitionKey,
                RowKey = Guid.NewGuid().ToString(),
                Title = item.Title,
                Content = item.Content,
                Link = item.Link
            };

            AddTransaction(feedObject, TableTransactionActionType.Add);
        }
    }

    public void Delete(string id)
    {
        var entity = new FeedItemAzureTableObject
        {
            PartitionKey = MentoringPartitionKey,
            RowKey = id,
        };

        AddTransaction(entity, TableTransactionActionType.Delete);
    }

    public void DeleteRange(IEnumerable<string> ids)
    {
        foreach (var id in ids)
        {
            var entity = new FeedItemAzureTableObject
            {
                PartitionKey = MentoringPartitionKey,
                RowKey = id,
            };

            AddTransaction(entity, TableTransactionActionType.Delete);
        }
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
    {
        var items = TableClient.QueryAsync<FeedItemAzureTableObject>(
            filter: $"PartitionKey eq '{MentoringPartitionKey}'",
            cancellationToken: cancellationToken);

        List<IFeedItem> feeds = new();

        await foreach (var item in items)
        {
            feeds.Add(_mapper.Map<FeedItemAzureTableObject, FeedItemDto>(item));
        }

        return (IEnumerable<T>)feeds;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var count = _transactionLog.Count;

        if (count > 0)
        {
            await TableClient.SubmitTransactionAsync(_transactionLog, cancellationToken);
            _transactionLog.Clear();
        }

        return count;
    }

    private void AddTransaction(FeedItemAzureTableObject item, TableTransactionActionType transactionType)
    {
        var action = new TableTransactionAction(transactionType, item);
        _transactionLog.Add(action);
    }
}