using System.Collections;

namespace Featurize.Repositories;
internal sealed class ProviderCollection : IProviderCollection
{
    private readonly List<IRepositoryProvider> _providers = new();

    public IRepositoryProvider? Get(string name)
    {
       return _providers
                .FirstOrDefault(x => x.Name
                .Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }

    public void Add(IRepositoryProvider provider)
    {
        _providers.Add(provider);
    }

    public IEnumerator<IRepositoryProvider> GetEnumerator() =>
        _providers.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Describes a ProviderCollection
/// </summary>
public interface IProviderCollection : IEnumerable<IRepositoryProvider>
{
    /// <summary>
    /// Gets the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    IRepositoryProvider? Get(string name);
    /// <summary>
    /// Adds the specified provider.
    /// </summary>
    /// <param name="provider">The provider.</param>
    void Add(IRepositoryProvider provider);
}
     