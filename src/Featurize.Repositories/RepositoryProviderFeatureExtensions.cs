namespace Featurize.Repositories;

/// <summary>
/// AddRepositoryProvider Extention methods on <see cref="IFeatureCollection"/>
/// </summary>
public static class RepositoryProviderFeatureExtensions
{
    /// <summary>
    /// Adds the Repository Provider Feature to the <see cref="IFeatureCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IFeatureCollection"/>.</param>
    /// <param name="options">The <see cref="RepositoryProviderOptions"/> to configure this feature.</param>
    /// <returns></returns>
    public static IFeatureCollection AddRepositoryProvider(this IFeatureCollection services, Action<RepositoryProviderOptions>? options = null)
        => services.AddWithOptions<RepositoryProviderFeature, RepositoryProviderOptions>(options);
}