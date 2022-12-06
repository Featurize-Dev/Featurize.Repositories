namespace Featurize.Repositories;

/// <summary>
/// Descibes a <see cref="IRepository{TEntity, TId}"/> that tracks versioned entities.
/// </summary>
/// <typeparam name="TEntity">The type of entity</typeparam>
/// <typeparam name="TId">The type of the Id.</typeparam>
public interface IVersionedRepository<TEntity, in TId> : IRepository<TEntity, TId>
    where TEntity : class
    where TId : struct
{
    /// <summary>
    /// Indicator for getting the latest version
    /// </summary>
    public const int AnyVersion = -1;


    /// <summary>
    /// Loads a specific verion of an entity.
    /// </summary>
    /// <param name="id">The id of the entity.</param>
    /// <param name="version">The version to load.</param>
    /// <returns></returns>
    Task<TEntity> LoadAsync(TId id, int version);
}