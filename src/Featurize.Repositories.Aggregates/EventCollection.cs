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

    public TId AggregateId { get; }
    public int Version => _events.Count;
    public int ExpectedVersion => Version + _uncomitted.Count;

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

    public void Append(IEvent e)
    {
        _uncomitted.Add(e);
    }

    public IEnumerable<IEvent> GetUncommittedEvents() => _uncomitted;

    public IEnumerator<IEvent> GetEnumerator() => _events.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}