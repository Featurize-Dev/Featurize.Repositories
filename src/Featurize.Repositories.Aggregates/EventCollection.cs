using System.Collections;

namespace Featurize.Repositories.Aggregates;

public class EventCollection<TAggregate, TId> : IReadOnlyCollection<IEvent>, IEnumerable, IEnumerable<IEvent>
    where TId : struct
{
    private readonly List<IEvent> _events = new();
    private readonly List<IEvent> _uncomitted = new();

    public TId AggregateId { get; }
    public int Version => _events.Count;
    public int ExpectedVersion => Version + _uncomitted.Count;

    public int Count => _events.Count;

    public EventCollection(TId aggregateId)
    {
        AggregateId = aggregateId;
    }

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