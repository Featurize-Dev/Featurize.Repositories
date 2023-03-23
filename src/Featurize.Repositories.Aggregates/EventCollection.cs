using System.Collections;

namespace Featurize.Repositories.Aggregates;

public class EventCollection<TId> : IEnumerable, IEnumerable<Event<TId>>
    where TId : struct
{
    private readonly List<Event<TId>> _events = new();

    public int Version => _events.Max(x => x.Version);

    public void Append(Event<TId> e)
    {
        _events.Add(e);
    }

    public IEnumerator<Event<TId>> GetEnumerator() => _events.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
