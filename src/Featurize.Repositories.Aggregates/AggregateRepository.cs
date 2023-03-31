namespace Featurize.Repositories.Aggregates;

internal class AggregateRepository<T, TId> : IRepository<T, TId>
    where T : class, IAggregate<T, TId>
    where TId : struct, IComparable<TId>
{
    private readonly IEntityRepository<Event<T, TId>, TId> _storage;

    public AggregateRepository(IEntityRepository<Event<T, TId>, TId> storage)
    {
        _storage = storage;
    }

    public async ValueTask<T?> FindByIdAsync(TId id)
    {
        var events = await _storage
           .Query
           .Where(x => x.AggregateId.Equals(id))
           .OrderBy(x => x.Version)
           .Select(x => x.Payload)
           .ToArrayAsync();

        var aggregate = T.Create(id);
        if (events.Any())
        {
            var eventCollection = new EventCollection<TId>(id, events);
            aggregate.LoadFromHistory(eventCollection);
        }
        return aggregate;
    }

    public async ValueTask SaveAsync(T entity)
    {
        //TODO: Check Version in database
        var version = entity.Version;
        foreach (var e in entity.GetUncommittedEvents())
        {
            version++;           
            await _storage.SaveAsync(Event<T, TId>.Create(entity.Id, version, e));
        }
    }

    public ValueTask DeleteAsync(TId id)
    {
        throw new NotImplementedException();
    }
}
