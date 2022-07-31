using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Infrastructure.AzureTable
{
    public class AzureTableRepository<T> : IRepositoryBase<T>
    {
        // Initiate Storage Account
        public CloudStorageAccount StorageAccount()
        {
            return CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"))
        }

        // Initiate the Table
        // Initiate a Partition Key
        // Initiate the Context. Ok there is no Context

        public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}