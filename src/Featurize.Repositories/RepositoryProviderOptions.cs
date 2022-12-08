using Featurize.Repositories.DefaultProvider;
using System.ComponentModel.DataAnnotations;

namespace Featurize.Repositories;

/// <summary>
/// Options to configure the RepositoryProviderFeature
/// </summary>
public sealed class RepositoryProviderOptions
{
    /// <summary>
    /// 
    /// </summary>
    public IProviderCollection Providers { get; } = new ProviderCollection()
    {
        new DefaultRepositoryProvider()
    };
    /// <summary>
    /// Collection that holds the registered repositories.
    /// </summary>
    public IRepositoryCollection Repositories { get; } = new RepositoryCollection();
    
    public void AddProvider(IRepositoryProvider provider)
    {
        Providers.Add(provider);
    }

    /// <summary>
    /// Adds a entity to use this provider
    /// </summary>
    /// <typeparam name="TEntity">a <see cref="IIdentifiable{TEntity, TId}"/> entory.</typeparam>
    /// <typeparam name="TId">The type of the id of this entity</typeparam>
    public void AddRepository<TEntity, TId>(Action<RepositoryOptions>? options = null)
        where TEntity : class, IIdentifiable<TEntity, TId>
        where TId: struct
    {
        var o = new RepositoryOptions();
        options?.Invoke(o);
        Repositories.Add<TEntity, TId>(o);
    }
}

