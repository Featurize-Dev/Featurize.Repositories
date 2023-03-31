using System.Collections;

namespace Featurize.Repositories.Aggregates;

/// <summary>
/// A Append only EventCollection
/// </summary>
/// <typeparam name="TId">The Type of the aggregateId</typeparam>
public class EventCollection<TId> : IReadOnlyCollection<IEvent>, IEnumerable, IEnumerable<IEvent>
    where TId : struct
{
    private readonly List<IEvent> _events = new();
    private readonly List<IEvent> _uncomitted = new();

    /// <summary>
    /// The Id of the aggregate
    /// </summary>
    public TId AggregateId { get; }
    /// <summary>
    /// The version of the aggregate
    /// </summary>
    public int Version => _events.Count;
    /// <summary>
    /// The expected version after save.
    /// </summary>
    public int ExpectedVersion => Version + _uncomitted.Count;

    /// <summary>
    /// The total number event comitted.
    /// </summary>
    public int Count => _events.Count;

    /// <summary>
    /// Creates a new EventCollection for an AggregateId
    /// </summary>
    /// <param name="aggregateId"></param>
    public EventCollection(TId aggregateId)
    {
        AggregateId = aggregateId;
    }

    /// <summary>
    /// Creates a new Collections for an aggregate id with events
    /// </summary>
    /// <param name="aggregateId">The id Of the aggregate</param>
    /// <param name="events">The events in this collection</param>
    public EventCollection(TId aggregateId, IEnumerable<IEvent> events)
    {
        AggregateId = aggregateId;
        _events = events.ToList();
    }

    /// <summary>
    /// Append a event to the collection.
    /// </summary>
    /// <param name="e"></param>
    public void Append(IEvent e)
    {
        _uncomitted.Add(e);
    }

    /// <summary>
    /// Get the uncommitted events
    /// </summary>
    /// <returns>Events that are uncommitted.</returns>
    public IEnumerable<IEvent> GetUncommittedEvents() => _uncomitted;

    /// <summary>
    /// Iterates over the comitted events
    /// </summary>
    /// <returns>Events that are committed</returns>
    public IEnumerator<IEvent> GetEnumerator() => _events.GetEnumerator();

    /// <summary>
    /// Iterates over the comitted events
    /// </summary>
    /// <returns>Events that are committed</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}