using Microsoft.EntityFrameworkCore;

namespace Featurize.Repositories.EntityFramework;

/// <summary>
/// A Entity Framework repository.
/// </summary>
/// <typeparam name="TContext">The DbContext the entity belongs to.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the indentifier.</typeparam>
public sealed class EntityFrameworkRepository<TContext, TEntity, TId> : IEntityRepository<TEntity, TId>
    where TContext : DbContext
    where TEntity : class, IIdentifiable<TEntity, TId>
    where TId : struct
{
    private readonly DbSet<TEntity> _collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkRepository{TEntity, TId}"/> class.
    /// </summary>
    /// <param name="context"></param>
    public EntityFrameworkRepository(TContext context)
    {
        Context = context;
        _collection = context.Set<TEntity>();
    }

    /// <summary>
    /// The <see cref="DbContext"/> this repository is using.
    /// </summary>
    public TContext Context { get; }

    /// <inheritdoc />
    public IQuery<TEntity> Query => new EntityFrameworkQuery<TEntity>(_collection.AsQueryable());

    /// <inheritdoc />
    public async ValueTask DeleteAsync(TId id)
    {
        var result = await _collection.FindAsync(id);
        if(result != null) 
        { 
            _collection.Remove(result);
            Context.SaveChanges();
        }
    }

    /// <inheritdoc />
    public ValueTask<TEntity?> FindByIdAsync(TId id)
    {
        return _collection.FindAsync(id);
    }

    /// <inheritdoc />
    public ValueTask SaveAsync(TEntity entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
            _collection.Add(entity);

        Context.SaveChanges();
        
        return ValueTask.CompletedTask;
    }
}
