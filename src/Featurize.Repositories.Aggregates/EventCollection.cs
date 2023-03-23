using System.Collections;

namespace Featurize.Repositories.Aggregates;

public class EventCollection<TAggregate, TId> : IEnumerable, IEnumerable<Event<TAggregate, TId>>
    where TId : struct
{
    private readonly List<Event<TAggregate, TId>> _events = new();

    public int Version => _events.Max(x => x.Version);

    public void Append(Event<TAggregate, TId> e)
    {
        _events.Add(e);
    }

    public IEnumerator<Event<TAggregate, TId>> GetEnumerator() => _events.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
