using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.InMemory;

/// <summary>
/// Default provider for providing default in memory repositories.
/// </summary>
public sealed class InMemoryRepositoryProvider : IRepositoryProvider
{
    public static string DefaultName => "InMemory";

    public bool IsConfigured => true;

    public string Name => DefaultName;

    /// <summary>
    /// Registerd the services required by this provider.
    /// </summary>
    /// <param name="services">The service collection <see cref="IServiceCollection"/>.</param>
    public void ConfigureProvider(IServiceCollection services)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="info"></param>
    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var implementationType = typeof(InMemoryRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);

        services.AddSingleton(serviceType, implementationType);
    }
}
