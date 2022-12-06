namespace Featurize.Repositories;

/// <summary>
/// Descibes a collection that hold <see cref="RepositoryInfo"/>
/// </summary>
public interface IRepositoryCollection : IEnumerable<RepositoryInfo>
{
    /// <summary>
    /// The number of elements in the collection.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Adds a <see cref="RepositoryInfo"/> to the collection.
    /// </summary>
    /// <param name="item"></param>
    public void Add(RepositoryInfo item);
}

/// <summary>
/// Extension methods to extend the <see cref="IRepositoryCollection"/>
/// </summary>
public static class RepositoryCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public static void Add<TEntity, TId>(this IRepositoryCollection collection, RepositoryOptions options)
        where TEntity : class, IIdentifiable<TEntity, TId>
        where TId : struct =>
        collection.Add(new(typeof(TEntity), typeof(TId), options));
}
