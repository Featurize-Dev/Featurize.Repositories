namespace Featurize.Repositories;


/// <summary>
/// Descibes a basic repository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the id for this entity.</typeparam>
public interface IRepository<TEntity, in TId>
    where TEntity : class
    where TId : struct
{
    /// <summary>
    /// Finds an entity with a specified id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <returns>The entity if found.</returns>
    ValueTask<TEntity?> FindByIdAsync(TId id);

    /// <summary>
    /// Saves the entity in the underlying storage.
    /// </summary>
    /// <param name="entity">The entity to save.</param>
    ValueTask SaveAsync(TEntity entity);

    /// <summary>
    /// Removes a entity from the underlying storage.
    /// </summary>
    /// <param name="id">Id of the entity to remove</param>    
    ValueTask DeleteAsync(TId id);
}
