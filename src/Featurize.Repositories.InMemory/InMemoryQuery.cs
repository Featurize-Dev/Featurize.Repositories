using System.Linq.Expressions;

namespace Featurize.Repositories.InMemory;

internal sealed class InMemoryQuery<TEntity> : IQuery<TEntity>, IQueryProvider
{
    private readonly IQueryable<TEntity> _queryable;

    public InMemoryQuery(IQueryable<TEntity> queryable)
    {
        _queryable = queryable;
    }

    public ValueTask<bool> AnyAsync()
    {
        return ValueTask.FromResult(_queryable.Any());
    }

    public ValueTask<int> CountAsync()
    {
        return ValueTask.FromResult(_queryable.Count());
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return _queryable.Provider.CreateQuery(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return _queryable.Provider.CreateQuery<TElement>(expression);
    }

    public object? Execute(Expression expression)
    {
        return _queryable.Provider.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _queryable.Provider.Execute<TResult>(expression);
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        foreach (var item in _queryable)
        {
            yield return item;
        }
    }

    public IQuery<TEntity> Skip(int count)
    {
        return new InMemoryQuery<TEntity>(_queryable.Skip(count));
    }

    public IQuery<TEntity> Take(int count)
    {
        return new InMemoryQuery<TEntity>(_queryable.Take(count));
    }

    public IQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return new InMemoryQuery<TEntity>(_queryable.Where(predicate));
    }
}
