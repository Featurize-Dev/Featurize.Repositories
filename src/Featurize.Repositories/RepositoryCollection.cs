using System.Collections;

namespace Featurize.Repositories;

/// <summary>
/// A collection for repoesitory information.
/// </summary>
public sealed class RepositoryCollection : IRepositoryCollection
{
    private readonly HashSet<RepositoryInfo> _items = new();
    /// <summary>
    /// The number of Items in this collection.
    /// </summary>
    public int Count => _items.Count;

    /// <summary>
    /// Adds an <see cref="RepositoryInfo"/> to the collection.
    /// </summary>
    /// <param name="repositoryInfo">a <see cref="RepositoryInfo"/>.</param>
    public void Add(RepositoryInfo repositoryInfo)
    {
        ArgumentNullException.ThrowIfNull(repositoryInfo, nameof(repositoryInfo));
        _items.Add(repositoryInfo);
    }

    /// <summary>
    /// To iterate through this instance.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<RepositoryInfo> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
