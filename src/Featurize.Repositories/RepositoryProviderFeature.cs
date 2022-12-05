using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories;

internal sealed class RepositoryProviderFeature :
    IFeatureWithOptions<RepositoryProviderFeature, RepositoryProviderOptions>,
    IServiceCollectionFeature
{
    private RepositoryProviderFeature(RepositoryProviderOptions options) => Options = options;

    public RepositoryProviderOptions Options { get; }

    public static RepositoryProviderFeature Create(RepositoryProviderOptions config)
    {
        return new RepositoryProviderFeature(config);
    }

    public void Configure(IServiceCollection services)
    {
        foreach (var item in Options.Repositories)
        {
            var implementationType = Options.Provider.MakeImplementationType(item.EntityType, item.IdType);
            var serviceTypes = Options.Provider.MakeServiceTypes(item.EntityType, item.IdType);

            foreach (var serviceType in serviceTypes)
            {
                services.AddScoped(serviceType, implementationType);
            }   
        }
    }
}
