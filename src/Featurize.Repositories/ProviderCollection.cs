using System.Collections;

namespace Featurize.Repositories;
public sealed class ProviderCollection : IProviderCollection
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

public interface IProviderCollection : IEnumerable<IRepositoryProvider>
{
    IRepositoryProvider? Get(string name);
    void Add(IRepositoryProvider provider);
}
     