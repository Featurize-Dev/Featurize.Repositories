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
    /// The EventCollection for this aggregate
    /// </summary>
    EventCollection<TId> Events { get; }
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
}

/// <summary>
/// A default base implementation of an AggregateRoot.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
/// <typeparam name="TId">The Type of the identifier.</typeparam>
public abstract class AggregateRoot<TAggregate, TId>
    where TId : struct
{
    /// <inheritdoc />
    public TId Id { get; private set; }
    /// <inheritdoc />
    public EventCollection<TId> Events { get; private set; }
    /// <inheritdoc />
    protected AggregateRoot(TId id)
    {
        Id = id;
        Events = new EventCollection<TId>(id);
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
        if (isNew) Events.Append(e);
    }

    private void Apply(IEvent e)
    {
        this.AsDynamic().When(e);
    }
    /// <inheritdoc />
    public void LoadFromHistory(EventCollection<TId> events)
    {
        Events = events;
        foreach (var e in events)
        {
            RecordEvent(e, false);
        }
    }
}