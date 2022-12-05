using System.Collections;

namespace Featurize.Repositories;

public sealed class RepositoryCollection : IRepositoryCollection
{
    private readonly HashSet<RepositoryInfo> _items = new();
    public int Count => _items.Count();

    public void Add(RepositoryInfo repositoryInfo)
    {
        ArgumentNullException.ThrowIfNull(repositoryInfo, nameof(repositoryInfo));
        _items.Add(repositoryInfo);
    }

    public IEnumerator<RepositoryInfo> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
