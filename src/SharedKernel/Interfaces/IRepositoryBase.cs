namespace RssFeeder.SharedKernel.Interfaces;

public interface IRepositoryBase<T> where T : class//, IFeedItem
{
    void Add(T entity);

    void AddRange(IEnumerable<T> entities);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    // Save the Changes
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
}