using Featurize.Repositories.DefaultProvider;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

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
    }

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
        //Options.Provider.ConfigureProvider(services);
        foreach (var info in Options.Repositories)
        {
            var providerName = info.Options.GetProviderName();

            if(string.IsNullOrEmpty(providerName))
            {
                providerName = DefaultRepositoryProvider.DefaultName;
            }

            var provider = Options.Providers.Get(providerName);
                        
            if (provider == null)
                throw new Exception($"No provider registerd with name: '{providerName}'.");

            if(!provider!.IsConfigured)
            {
                provider.ConfigureProvider(services);
            }
            
            provider.ConfigureRepository(services, info);
        }
    }
}
