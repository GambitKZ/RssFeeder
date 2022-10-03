using AutoMapper;
using Azure.Data.Tables;
using RssFeeder.Application.Common.Models;
using RssFeeder.Infrastructure.AzureTable.Models;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Infrastructure.AzureTable;

public class AzureTableRepository<T> : IRepositoryBase<T> where T : class, IFeedItem
{
    // TODO: Move from constant to Property
    private const string MentoringPartitionKey = "MentoringProgram";

    private readonly IMapper _mapper;
    private List<TableTransactionAction> transactionLog = new();

    public AzureTableRepository(string connectionString, string tableName, IMapper mapper)
    {
        var tableServiceClient = new TableServiceClient(connectionString);

        // TODO: Pass the Table name not in Constructor?
        tableServiceClient.CreateTableIfNotExists(tableName);
        TableClient = tableServiceClient.GetTableClient(tableName);

        _mapper = mapper;
    }

    private TableClient TableClient { get; }

    public void Add(T entity)
    {
        var feedObject = new FeedItemAzureTableDto
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
            Add(item);
        }
    }

    public void Delete(string id)
    {
        var entity = new FeedItemAzureTableDto
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
            var entity = new FeedItemAzureTableDto
            {
                PartitionKey = MentoringPartitionKey,
                RowKey = id,
            };

            AddTransaction(entity, TableTransactionActionType.Delete);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = TableClient.QueryAsync<FeedItemAzureTableDto>(
            filter: $"PartitionKey eq '{MentoringPartitionKey}'",
            cancellationToken: cancellationToken);

        List<IFeedItem> feeds = new();

        await foreach (var item in items)
        {
            feeds.Add(_mapper.Map<FeedItemAzureTableDto, FeedItemRepositoryDto>(item));
        }

        return (IEnumerable<T>)feeds;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var count = transactionLog.Count;

        if (count > 0)
        {
            await TableClient.SubmitTransactionAsync(transactionLog, cancellationToken);
            transactionLog.Clear();
        }

        return count;
    }

    private void AddTransaction(FeedItemAzureTableDto item, TableTransactionActionType transactionType)
    {
        var action = new TableTransactionAction(transactionType, item);
        transactionLog.Add(action);
    }
}