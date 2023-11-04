using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.EntityFramework;

/// <summary>
/// 
/// </summary>
public class EntityFrameworkRepositoryProvider : IRepositoryProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public EntityFrameworkRepositoryProvider(EntityFrameworkRepositoryProviderOptions options)
    {
        Name = options.Name;
        Options = options;
    }

    /// <summary>
    /// Default name of this Repository Provider.
    /// </summary>
    public static string DefaultName => "EntityFramework";

    /// <inheritdoc />
    public string Name { get; }
    /// <inheritdoc />
    public bool IsConfigured { get; }

    /// <summary>
    /// Options for this provider instance
    /// </summary>
    public EntityFrameworkRepositoryProviderOptions Options { get; }

    /// <inheritdoc />
    public void ConfigureProvider(IServiceCollection services)
    {
        
    }

    /// <inheritdoc />
    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var repositoryType = typeof(IRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        var implType = typeof(EntityFrameworkRepository<,,>).MakeGenericType(GetContextType(info.Options), info.EntityType, info.IdType);
        
        services.AddTransient(serviceType, implType);
        services.AddTransient(repositoryType, implType);
    }

    private static Type GetContextType(RepositoryOptions options)
    {
        if(options.TryGetValue("context", out object? value))
        {
            return (Type)value;
        }

        throw new InvalidOperationException($"Context not set.");
    }
}


public class EntityFrameworkRepositoryProviderOptions
{
    public string Name { get; set; } = EntityFrameworkRepositoryProvider.DefaultName;
}