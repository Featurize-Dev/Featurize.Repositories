using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories.MongoDB;
public class MongoEntityRepository<TEntity, TId> : IEntityRepository<TEntity, TId>
    where TEntity : class, IIdentifiable<TEntity, TId>
    where TId : struct
{
    private readonly IMongoCollection<TEntity> _collection;
        
    public MongoEntityRepository(IMongoCollection<TEntity> collection)
    {
        _collection = collection;
    }

    public IQuery<TEntity> Query => new MongoQuery<TEntity>(_collection.AsQueryable());

    public async ValueTask DeleteAsync(TId id)
    {
        await _collection.DeleteOneAsync(
           filter: Builders<TEntity>.Filter.Eq("_id", id),
           options: new DeleteOptions());
    }

    public async ValueTask<TEntity?> FindByIdAsync(TId id)
    {
        var cursor = await _collection.FindAsync(filter: Builders<TEntity>.Filter.Eq("_id", id));
        return await cursor.FirstOrDefaultAsync();
    }

    public async ValueTask SaveAsync(TEntity entity)
    {
        await _collection.ReplaceOneAsync(
            filter: Builders<TEntity>.Filter.Eq("_id", TEntity.Identify(entity)),
            options: new ReplaceOptions { IsUpsert = true },
            replacement: entity);
    }
}
