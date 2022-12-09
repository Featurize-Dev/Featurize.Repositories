using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.DefaultProvider;
/// <summary>
/// The Default Repository provider
/// </summary>
/// <seealso cref="Featurize.Repositories.IRepositoryProvider" />
public class DefaultRepositoryProvider : IRepositoryProvider
{
    /// <summary>
    /// Gets the default name.
    /// </summary>
    /// <value>
    /// The default name.
    /// </value>
    public static string DefaultName => "Default";

    /// <summary>
    /// Name of the Provider.
    /// </summary>
    public string Name => DefaultName;

    /// <summary>
    /// Indicates if the provider is configured
    /// </summary>
    public bool IsConfigured => true;

    /// <summary>
    /// Configures services required for this provider.
    /// </summary>
    /// <param name="services">The service collection <see cref="IServiceCollection" />.</param>
    public void ConfigureProvider(IServiceCollection services)
    {
        
    }

    /// <summary>
    /// Configures the services for the repository.
    /// </summary>
    /// <param name="services">The service collection <see cref="IServiceCollection" />.</param>
    /// <param name="info">The <see cref="RepositoryInfo"/> to register the repository.</param>
    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        services.AddTransient(serviceType, c => throw new ArgumentException("No Provider Configured."));
    }
}
