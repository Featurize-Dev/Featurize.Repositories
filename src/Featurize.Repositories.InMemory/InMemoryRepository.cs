namespace Featurize.Repositories.InMemory;

/// <summary>
/// Default in memory repository.
/// </summary>
/// <typeparam name="TEntity">The type of the Entity</typeparam>
/// <typeparam name="TId">The type of the Id.</typeparam>
public sealed class InMemoryRepository<TEntity, TId> : IEntityRepository<TEntity, TId>
    where TEntity : class, IIdentifiable<TEntity, TId>
    where TId : struct
{
    private readonly Dictionary<TId, TEntity> _items = new();

    /// <summary>
    /// Allows to query this repository
    /// </summary>
    public IQuery<TEntity> Query => new InMemoryQuery<TEntity>(_items.Values.AsQueryable());

    /// <summary>
    /// Deletes an entity from the collection.
    /// </summary>
    /// <param name="id">The id of the entity to be deleted.</param>
    /// <returns></returns>
    public ValueTask DeleteAsync(TId id)
    {
        _items.Remove(id);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Search the repository for an entity with the given id.
    /// </summary>
    /// <param name="id">The id of the entity to search</param>
    /// <returns>Returns a entity if found.</returns>
    public ValueTask<TEntity?> FindByIdAsync(TId id)
    {
        if (_items.TryGetValue(id, out var item))
        {
            return ValueTask.FromResult<TEntity?>(item);
        }
        return ValueTask.FromResult(default(TEntity));
    }

    /// <summary>
    /// Saves a entity in the collection.
    /// </summary>
    /// <param name="entity">the entity.</param>
    public ValueTask SaveAsync(TEntity entity)
    {
        _items[TEntity.Identify(entity)] = entity;
        return ValueTask.CompletedTask;
    }
}