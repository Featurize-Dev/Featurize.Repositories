using Featurize.Repositories.DefaultProvider;

namespace Featurize.Repositories;

/// <summary>
/// Options to configure the RepositoryProviderFeature
/// </summary>
public sealed class RepositoryProviderOptions
{
    /// <summary>
    /// Collection that holds the registered repositories.
    /// </summary>
    public IRepositoryCollection Repositories { get; } = new RepositoryCollection();

    /// <summary>
    /// The Provider that will create the repositories.
    /// </summary>
    public IRepositoryProvider Provider { get; set; } = new DefaultRepositoryProvider();
}
