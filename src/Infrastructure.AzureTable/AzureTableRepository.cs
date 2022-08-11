using AutoMapper;
using Azure.Data.Tables;
using RssFeeder.Infrastructure.AzureTable.Models;
using RssFeeder.SharedKernel.Interfaces;
using RssFeeder.SharedKernel.Models;

namespace RssFeeder.Infrastructure.AzureTable;

public class AzureTableRepository<T> : IRepositoryBase<T> where T : class, IFeedItem
{
    private readonly IMapper _mapper;
    private List<TableTransactionAction> _transactionLog = new();

    public AzureTableRepository(string connectionString, string tableName, IMapper mapper)
    {
        var tableServiceClient = new TableServiceClient(connectionString);

        TableClient = tableServiceClient.GetTableClient(tableName: tableName);
        _mapper = mapper;
    }

    private TableClient TableClient { get; }

    public void Add(T entity)
    {
        AddTransaction(entity, TableTransactionActionType.Add);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        foreach (var item in entities)
        {
            AddTransaction(item, TableTransactionActionType.Add);
        }
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
    {
        var items = TableClient.QueryAsync<FeedItemAzureTableObject>(
            filter: "PartitionKey eq 'MentoringProgram'",
            cancellationToken: cancellationToken);

        List<IFeedItem> feeds = new();

        await foreach (var item in items)
        {
            feeds.Add(_mapper.Map<FeedItemAzureTableObject, FeedItem>(item));
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

    private void AddTransaction(T item, TableTransactionActionType transactionType)
    {
        var feedObject = new FeedItemAzureTableObject
        {
            PartitionKey = "MentoringProgram",
            RowKey = Guid.NewGuid().ToString(),
            Title = item.Title,
            Content = item.Content,
            Link = item.Link
        };

        var action = new TableTransactionAction(transactionType, feedObject);
        _transactionLog.Add(action);
    }
}