﻿namespace RssFeeder.SharedKernel.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    void Add(T entity);

    void AddRange(IEnumerable<T> entities);

    void Delete(string id);

    void DeleteRange(IEnumerable<string> ids);

    // Save the Changes
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
}