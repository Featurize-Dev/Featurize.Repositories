namespace Featurize.Repositories.DefaultProvider;

public class DefaultRepository<TEntity, TId> : IStateRepository<TEntity, TId>
    where TEntity : class, IIdentifiable<TEntity, TId>
    where TId : struct
{
    private Dictionary<TId, TEntity> _items = new();

    public IQuery<TEntity> Query => new DefaultQuery<TEntity>(_items.Values.AsQueryable());

    public ValueTask DeleteAsync(TId key)
    {
        _items.Remove(key);
        return ValueTask.CompletedTask;
    }   

    public ValueTask<TEntity?> FindByIdAsync(TId key)
    {
        if (_items.TryGetValue(key, out var item))
        {
            return ValueTask.FromResult<TEntity?>(item);
        }
        return ValueTask.FromResult(default(TEntity));
    }

    public ValueTask SaveAsync(TEntity entity)
    {
        _items[TEntity.Identify(entity)] = entity;
        return ValueTask.CompletedTask;
    }

    public void Enqueue(TId identity, Func<TEntity> create)
    {
        _items[identity] = create();
    }

    public void Enqueue(TId identity, Func<TEntity, TEntity?> update)
    {
        if (_items.TryGetValue(identity, out var item))
        {
            _items[identity] = update(item)!;
        }
    }

    public Task CommitAsync()
    {
        return Task.CompletedTask;
    }
}