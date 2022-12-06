namespace Featurize.Repositories;

/// <summary>
/// Extension methods for <see cref="RepositoryProviderOptions"/>.
/// </summary>
public static class RepositoryProviderOptionsExtensions
{
    /// <summary>
    /// Adds a entity to use this provider
    /// </summary>
    /// <typeparam name="TEntity">a <see cref="IIdentifiable{TEntity, TId}"/> entory.</typeparam>
    /// <typeparam name="TId">The type of the id of this entity</typeparam>
    public static void AddRepository<TEntity, TId>(this RepositoryProviderOptions providerOptions, Action<RepositoryOptions>? options = null)
        where TEntity : class, IIdentifiable<TEntity, TId>
        where TId : struct
    {
        var o = new RepositoryOptions();
        options?.Invoke(o);
        providerOptions.Repositories.Add<TEntity, TId>(o);
    }
}
