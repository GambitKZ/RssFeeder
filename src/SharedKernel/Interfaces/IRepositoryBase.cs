namespace RssFeeder.SharedKernel.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    void Add(T entity);

    // Add number of Feeds
    void AddRange(IEnumerable<T> entities);

    // Remove a Feed
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    // Remove Number of Feeds
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    // Save the Changes
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}