namespace Featurize.Repositories;

public interface IBufferedRepository<TEntity, TId>
    where TEntity : class
    where TId : struct
{
    void Enqueue(TId identity, Func<TEntity> create);

    void Enqueue(TId identity, Func<TEntity, TEntity?> update);

    Task CommitAsync();
}
