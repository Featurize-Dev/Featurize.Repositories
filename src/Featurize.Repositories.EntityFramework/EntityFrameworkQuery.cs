using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Featurize.Repositories.EntityFramework;

internal sealed class EntityFrameworkQuery<TEntity> : IQuery<TEntity>, IQueryProvider
{
    private readonly IQueryable<TEntity> _queryable;

    public EntityFrameworkQuery(IQueryable<TEntity> queryable)
    {
        _queryable = queryable;
    }
    public async ValueTask<bool> AnyAsync()
    {
        return await _queryable.AnyAsync();
    }

    public async ValueTask<int> CountAsync()
    {
        return await _queryable.CountAsync();
    }

    public IQueryable CreateQuery(Expression expression)
        => _queryable.Provider.CreateQuery(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => _queryable.Provider.CreateQuery<TElement>(expression);

    public object? Execute(Expression expression)
        => _queryable.Provider.Execute(expression);

    public TResult Execute<TResult>(Expression expression)
        => _queryable.Provider.Execute<TResult>(expression);

    public async IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        foreach(var item in _queryable)
        {
            yield return item;
        }
    }

    public IQuery<TEntity> Skip(int count)
        => new EntityFrameworkQuery<TEntity>(_queryable.Skip(count));

    public IQuery<TEntity> Take(int count)
        => new EntityFrameworkQuery<TEntity>(_queryable.Take(count));

    public IQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        => new EntityFrameworkQuery<TEntity>(_queryable.Where(predicate));
}
