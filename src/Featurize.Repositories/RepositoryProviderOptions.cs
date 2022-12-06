using Featurize.Repositories.DefaultProvider;

namespace Featurize.Repositories;

/// <summary>
/// Options to configure the RepositoryProviderFeature
/// </summary>
public class RepositoryProviderOptions
{
    /// <summary>
    /// Collection that holds the registered repositories.
    /// </summary>
    public IRepositoryCollection Repositories { get; } = new RepositoryCollection();

    /// <summary>
    /// The Provider that will create the repositories.
    /// </summary>
    public IRepositoryProvider Provider { get; set; } = new DefaultRepositoryProvider();
    
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

/// <summary>
/// Generic options collection
/// </summary>
public class RepositoryOptions
{
    private readonly Dictionary<string, string> _items = new();

    /// <summary>
    /// Set additional options for a repository.
    /// </summary>
    /// <param name="key">The key of the setting.</param>
    /// <param name="value">The value of the setting.</param>
    public void Set(string key, string value)
    {
        _items[key] = value;
    }

    /// <summary>
    /// Get additional option for a repository
    /// </summary>
    /// <param name="key"></param>
    /// <returns>Returns the value </returns>
    public string Get(string key) { 
        return _items[key]; 
    }
}