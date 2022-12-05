namespace Featurize.Repositories;

public interface IRepository<TEntity, in TId>
    where TEntity : class
    where TId : struct
{
    ValueTask<TEntity?> FindByIdAsync(TId key);
    ValueTask SaveAsync(TEntity entity);
    ValueTask DeleteAsync(TId key);
}
