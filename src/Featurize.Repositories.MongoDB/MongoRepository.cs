using MongoDB.Driver;

namespace Featurize.Repositories.MongoDB;
/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <seealso cref="Featurize.Repositories.IEntityRepository&lt;TEntity, TId&gt;" />
public sealed class MongoEntityRepository<TEntity, TId> : IEntityRepository<TEntity, TId>
    where TEntity : class, IIdentifiable<TEntity, TId>
    where TId : struct
{
    private readonly IMongoCollection<TEntity> _collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoEntityRepository{TEntity, TId}"/> class.
    /// </summary>
    /// <param name="collection">The collection.</param>
    public MongoEntityRepository(IMongoCollection<TEntity> collection)
    {
        _collection = collection;
    }
    /// <summary>
    /// Gets the collection.
    /// </summary>
    /// <value>
    /// The collection.
    /// </value>
    public IMongoCollection<TEntity> Collection { get { return _collection; } }
    /// <summary>
    /// Gets the query.
    /// </summary>
    /// <value>
    /// The query.
    /// </value>
    public IQuery<TEntity> Query => new MongoQuery<TEntity>(_collection.AsQueryable());
    /// <summary>
    /// Removes a entity from the underlying storage.
    /// </summary>
    /// <param name="id">Id of the entity to remove</param>
    /// <returns></returns>
    public async ValueTask DeleteAsync(TId id)
    {
        await _collection.DeleteOneAsync(
           filter: Builders<TEntity>.Filter.Eq("_id", id),
           options: new DeleteOptions());
    }
    /// <summary>
    /// Finds the by identifier asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async ValueTask<TEntity?> FindByIdAsync(TId id)
    {
        var cursor = await _collection.FindAsync(filter: Builders<TEntity>.Filter.Eq("_id", id));
        return await cursor.FirstOrDefaultAsync();
    }
    /// <summary>
    /// Saves the entity in the underlying storage.
    /// </summary>
    /// <param name="entity">The entity to save.</param>
    /// <returns></returns>
    public async ValueTask SaveAsync(TEntity entity)
    {
        await _collection.ReplaceOneAsync(
            filter: Builders<TEntity>.Filter.Eq("_id", TEntity.Identify(entity)),
            options: new ReplaceOptions { IsUpsert = true },
            replacement: entity);
    }
}
