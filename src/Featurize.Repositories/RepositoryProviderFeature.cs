using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories;

/// <summary>
/// A Repository provider feature.
/// </summary>
public sealed class RepositoryProviderFeature :
    IFeatureWithOptions<RepositoryProviderFeature, RepositoryProviderOptions>,
    IServiceCollectionFeature
{
    private RepositoryProviderFeature(RepositoryProviderOptions options)
    {
        Options = options;
        Name = options.Provider.GetType().Name;
    }

    /// <summary>
    /// Name of the provider
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// The <see cref="RepositoryProviderOptions"/> used to configure this instance.
    /// </summary>
    public RepositoryProviderOptions Options { get; }

    /// <summary>
    /// Creates a <see cref="RepositoryProviderFeature"/> with the <see cref="RepositoryProviderOptions"/>.
    /// </summary>
    /// <param name="config">The <see cref="RepositoryProviderOptions"/>.</param>
    /// <returns>A <see cref="RepositoryProviderFeature"/>.</returns>
    public static RepositoryProviderFeature Create(RepositoryProviderOptions config)
    {
        return new RepositoryProviderFeature(config);
    }

    /// <summary>
    /// Configures the <see cref="RepositoryProviderFeature"/>.
    /// </summary>
    /// <param name="services"></param>
    public void Configure(IServiceCollection services)
    {
        Options.Provider.ConfigureProvider(services);
        foreach (var info in Options.Repositories)
        {
            Options.Provider.ConfigureRepository(services, info);
        }
    }
}
