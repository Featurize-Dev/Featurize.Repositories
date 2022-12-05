using Featurize.Repositories.DefaultProvider;

namespace Featurize.Repositories;

public class RepositoryProviderOptions
{
    public IRepositoryProvider Provider { get; } = new DefaultRepositoryProvider();
    public IRepositoryCollection Repositories { get; } = new RepositoryCollection();
}
