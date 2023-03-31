namespace Featurize.Repositories.Aggregates;

/// <summary>
/// Marks an class as an AggregateRoot
/// </summary>
/// <typeparam name="TSelf">The Type of the class</typeparam>
/// <typeparam name="TId">The Type of the Id for the aggregate</typeparam>
public interface IAggregate<TSelf, TId> : IIdentifiable<TSelf, TId>
    where TSelf : class
    where TId : struct
{
    /// <summary>
    /// The Identifier of the AggregateRoot
    /// </summary>
    TId Id { get; }

    /// <summary>
    /// The verion of the aggregate.
    /// </summary>
    int Version { get; }
    /// <summary>
    /// The expected version of the aggregate after the save.
    /// </summary>
    int ExpectedVersion { get; }
    /// <summary>
    /// Factory method to create a new Aggregate.
    /// </summary>
    /// <param name="id">The Id of the Aggregate.</param>
    /// <returns>Returns a new instance of this Aggregate.</returns>
    static abstract TSelf Create(TId id);

    /// <summary>
    /// Loads the events for this aggregate
    /// </summary>
    /// <param name="events"></param>
    void LoadFromHistory(EventCollection<TId> events);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    EventCollection<TId> GetUncommittedEvents();
}

/// <summary>
/// A default base implementation of an AggregateRoot.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
/// <typeparam name="TId">The Type of the identifier.</typeparam>
public abstract class AggregateRoot<TAggregate, TId>
    where TId : struct
{
    private EventCollection<TId> _events;

    /// <inheritdoc />
    public TId Id { get; private set; }
    /// <inheritdoc />
    public int Version => _events.Version;
    /// <inheritdoc />
    public int ExpectedVersion => _events.ExpectedVersion;
    /// <inheritdoc />
    protected AggregateRoot(TId id)
    {
        Id = id;
        _events = new EventCollection<TId>(id);
    }
    /// <summary>
    /// Applies a new event to the EventCollection.
    /// </summary>
    /// <param name="e">The event to append.</param>
    public void RecordEvent(IEvent e)
    {
        ArgumentNullException.ThrowIfNull(e, nameof(e));
        ArgumentNullException.ThrowIfNull(Id, nameof(Id));

        RecordEvent(e, true);
    }

    private void RecordEvent(IEvent e, bool isNew)
    {
        Apply(e);
        if (isNew) _events.Append(e);
    }

    private void Apply(IEvent e)
    {
        this.AsDynamic().Apply(e);
    }
    /// <inheritdoc />
    public void LoadFromHistory(EventCollection<TId> events)
    {
        _events = events;
        foreach (var e in events)
        {
            RecordEvent(e, false);
        }
    }
    /// <inheritdoc />
    public EventCollection<TId> GetUncommittedEvents() => new(Id, _events.GetUncommittedEvents());
}