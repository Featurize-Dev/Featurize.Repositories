namespace Featurize.Repositories;

public interface IVersionedRepository<TEntity, in TId> : IRepository<TEntity, TId>
    where TEntity : class
    where TId : struct
{
    public const int AnyVersion = -1;

    Task<TEntity> LoadAsync(TId identity, int expectedVersion);
}