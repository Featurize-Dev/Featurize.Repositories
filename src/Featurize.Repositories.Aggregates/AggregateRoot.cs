namespace Featurize.Repositories.Aggregates;

public interface IAggregate<TSelf, TId> : IIdentifiable<TSelf, TId>
    where TSelf : class
    where TId : struct
{
    TId Id { get; }
    EventCollection<TId> Events { get; }
    static abstract TSelf Create(TId id);
    void LoadFromHistory(EventCollection<TId> events);
}

public abstract class AggregateRoot<TAggregate, TId>
    where TId : struct
{
    public TId Id { get; private set; }
    public EventCollection<TId> Events { get; private set; }
    protected AggregateRoot(TId id)
    {
        Id = id;
        Events = new EventCollection<TId>(id);
    }

    public void ApplyEvent(IEvent e)
    {
        ArgumentNullException.ThrowIfNull(e, nameof(e));
        ArgumentNullException.ThrowIfNull(Id, nameof(Id));

        ApplyEvent(e, true);
    }

    private void ApplyEvent(IEvent e, bool isNew)
    {
        Apply(e);
        if (isNew) Events.Append(e);
    }

    private void Apply(IEvent e)
    {
        this.AsDynamic().Apply(e);
    }

    public void LoadFromHistory(EventCollection<TId> events)
    {
        Events = events;
        foreach (var e in events)
        {
            ApplyEvent(e, false);
        }
    }
}