using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.DefaultProvider;

/// <summary>
/// A default RepositoryProvider.
/// </summary>
/// <seealso cref="Featurize.Repositories.IRepositoryProvider" />
public sealed class DefaultRepositoryProvider : IRepositoryProvider
{
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
    /// <param name="services">The service collection <see cref="IServiceCollection"/>.</param>
    /// <param name="info">The <see cref="RepositoryInfo"/>.</param>
/    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        services.AddTransient(serviceType, c => throw new ArgumentException("No Provider Configured."));
    }
}
