namespace Featurize.Repositories;

public interface IRepositoryCollection : IEnumerable<RepositoryInfo>
{
    public int Count { get; }
    public void Add(RepositoryInfo item);

    public void Add<TEntity, TId>() 
        where TEntity : class, IIdentifiable<TEntity, TId> 
        where TId : struct =>
        Add(new(typeof(TEntity), typeof(TId)));
}
