namespace Featurize.Repositories;

public static class RepositoryProviderFeatureExtensions
{
    public static IFeatureCollection AddRepositoryProvider(this IFeatureCollection services, Action<RepositoryProviderOptions> options)
        => services.AddWithOptions<RepositoryProviderFeature, RepositoryProviderOptions>(options);
}
