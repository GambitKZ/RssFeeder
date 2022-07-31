namespace RssFeeder.SharedKernel.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    // Add Feed
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    // Add number of Feed
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    // Remove a Feed
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    // Remove Number of Feed
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    // Save the Changes
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}