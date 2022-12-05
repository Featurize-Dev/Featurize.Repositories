namespace Featurize.Repositories;

public interface IIdentifiable<TEntity, TId>
    where TEntity : class
    where TId : struct
{
    static abstract TId Identify(TEntity entity);
}