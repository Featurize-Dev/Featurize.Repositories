using System.Linq.Expressions;

namespace Featurize.Repositories;

/// <summary>
/// Descibes a <see cref="IRepository{TEntity, TId}"/> that is Queriable.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the Id of the entity.</typeparam>
public interface IQueryableRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class
    where TId : struct
{
    /// <summary>
    /// 
    /// </summary>
    IQuery<TEntity> Query { get; }
}

/// <summary>
/// Descibes a type as IQuery
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IQuery<TEntity> : IAsyncEnumerable<TEntity>
{
    /// <summary>
    /// Determines whether a sequence contains any elements. 
    /// </summary>
    /// <returns></returns>
    ValueTask<bool> AnyAsync();

    /// <summary>
    /// Returns the number of elements in a sequence.
    /// </summary>
    /// <returns></returns>
    ValueTask<int> CountAsync();

    /// <summary>
    /// Filters a sequence of values based on a predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>An <see cref="IQuery{TEntity}"/> that contains elements from the input sequence that satisfy the condition specified by predicate.</returns>
    IQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
    /// </summary>
    /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
    /// <returns>An <see cref="IQuery{TEntity}"/> that contains elements that occur after the specified index in the input sequence.</returns>
    IQuery<TEntity> Skip(int count);

    /// <summary>
    /// Returns a specified number of contiguous elements from the start of a sequence.
    /// </summary>
    /// <param name="count">The number of elements to return.</param>
    /// <returns>An <see cref="IQuery{TEntity}"/> that contains the specified number of elements from the start of source.</returns>
    IQuery<TEntity> Take(int count);
}