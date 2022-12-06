using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories;

/// <summary>
/// Descibes a repository provider.
/// </summary>
public interface IRepositoryProvider
{
    /// <summary>
    /// Configures services required for this provider.
    /// </summary>
    /// <param name="services">The service collection <see cref="IServiceCollection"/>.</param>
    void ConfigureProvider(IServiceCollection services);

    /// <summary>
    /// Configures the services for the repository.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="info"></param>
    void ConfigureRepository(IServiceCollection services, RepositoryInfo info);
}
