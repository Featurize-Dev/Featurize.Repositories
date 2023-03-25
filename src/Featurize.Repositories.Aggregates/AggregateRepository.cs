using Featurize.Repositories.Aggregates.Publsher;

namespace Featurize.Repositories.Aggregates;

public class AggregateRepository<T, TId> : IRepository<T, TId>
    where T : class, IAggregate<T, TId>
    where TId : struct, IComparable<TId>
{
    private readonly IEntityRepository<Event<T, TId>, TId> _storage;
    private readonly IEventPublisher _publisher;

    public AggregateRepository(IEntityRepository<Event<T, TId>, TId> storage, IEventPublisher publisher)
    {
        _storage = storage;
        _publisher = publisher;
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
            if(e.Payload is { })
                await _publisher.Publish(e.Payload);
        }
    }

    public ValueTask DeleteAsync(TId id)
    {
        throw new NotImplementedException();
    }
}
