using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories;

internal sealed class RepositoryProviderFeature :
    IFeatureWithOptions<RepositoryProviderFeature, RepositoryProviderOptions>,
    IServiceCollectionFeature
{
    private RepositoryProviderFeature(RepositoryProviderOptions options)
    {
        Options = options;
        Name = options.Provider.GetType().Name;
    }

    public string Name { get; init; }

    public RepositoryProviderOptions Options { get; }

    public static RepositoryProviderFeature Create(RepositoryProviderOptions config)
    {
        return new RepositoryProviderFeature(config);
    }

    public void Configure(IServiceCollection services)
    {
        Options.Provider.ConfigureProvider(services);
        foreach (var info in Options.Repositories)
        {
            Options.Provider.ConfigureRepository(services, info);
        }
    }
}
