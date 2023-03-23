﻿namespace Featurize.Repositories.Aggregates;

public class AggregateRepository<T, TId> : IRepository<T, TId>
    where T : class, IAggregate<T, TId>
    where TId : struct, IComparable<TId>
{
    private readonly IEntityRepository<Event<TId>, TId> _storage;
    public AggregateRepository(IEntityRepository<Event<TId>, TId> storage)
    {
        _storage = storage;
    }

    public async ValueTask<T?> FindByIdAsync(TId id)
    {
        var events = await _storage
            .Query
            .Where(x => x.AggregateId.Equals(id))
            .OrderBy(x => x.Version)
            .ToListAsync();

        var aggregate = T.Create(id);
        aggregate.LoadFromHistory(events);
        return aggregate;
    }

    public async ValueTask SaveAsync(T entity)
    {
        //TODO: Check Version in database

        foreach(var e in entity.GetUncommittedChanges())
        {
            await _storage.SaveAsync(e);
        }
    }

    public ValueTask DeleteAsync(TId id)
    {
        throw new NotImplementedException();
    }
}
