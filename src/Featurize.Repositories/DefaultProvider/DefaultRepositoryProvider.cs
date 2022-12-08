using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.DefaultProvider;
public class DefaultRepositoryProvider : IRepositoryProvider
{
    public static string DefaultName => "Default";

    public string Name => DefaultName;

    public bool IsConfigured => true;

    public void ConfigureProvider(IServiceCollection services)
    {
        
    }

    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        services.AddTransient(serviceType, c => throw new ArgumentException("No Provider Configured."));
    }
}
