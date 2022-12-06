using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.DefaultProvider;
internal class DefaultRepositoryProvider : IRepositoryProvider
{
    public void ConfigureProvider(IServiceCollection services)
    {
        
    }

    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        services.AddTransient(serviceType, c => throw new ArgumentException("No Provider Configured."));
    }
}
