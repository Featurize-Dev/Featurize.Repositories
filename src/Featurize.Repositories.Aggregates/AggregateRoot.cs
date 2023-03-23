namespace Featurize.Repositories.Aggregates;

public interface IAggregate<TSelf, TId> : IIdentifiable<TSelf, TId>
    where TSelf : class
    where TId : struct
{
    TId Id { get; }
    int Version { get; }
    IEnumerable<Event<TId>> GetUncommittedChanges();
    void LoadFromHistory(IEnumerable<Event<TId>> events);
    static abstract TSelf Create(TId id);
}

public abstract class AggregateRoot<TAggregate, TId>
    where TId: struct
{
    private readonly EventCollection<TId> _events = new();
    public TId Id { get; private set; }
    public int Version { get; private set; }

    public int ExpectedVersion { get; private set; }

    protected AggregateRoot(TId id) { Id = id; Version = 0; }

    public IEnumerable<Event<TId>> GetUncommittedChanges()
    {
        return _events;
    }

    public void LoadFromHistory(IEnumerable<Event<TId>> events)
    {
        foreach (var e in events)
        {
            Version = e.Version;
            ApplyEvent(e, false);
        }
        ExpectedVersion = Version;
    }

    public void ApplyEvent(IEvent e)
    {
        ArgumentNullException.ThrowIfNull(e, nameof(e));
        ArgumentNullException.ThrowIfNull(Id, nameof(Id));

        ExpectedVersion += 1;
        ApplyEvent(new Event<TId>
        {
            AggregateId = Id,
            Version = ExpectedVersion,
            Payload = e
        }, true);
    }

    private void ApplyEvent(Event<TId> e, bool isNew)
    {
        Apply(e);
        if (isNew) _events.Append(e);
    }

    private void Apply(Event<TId> e)
    {
        this.AsDynamic().Apply(e.Payload);
    }
}
