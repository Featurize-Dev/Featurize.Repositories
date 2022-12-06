using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Featurize.Repositories.MongoDB;
internal sealed class MongoQuery<TState> : IQuery<TState>, IQueryProvider
{
    private readonly IMongoQueryable<TState> _queryable;

    public MongoQuery(IMongoQueryable<TState> queryable)
    {
        _queryable = queryable;
    }

    public Type ElementType => _queryable.ElementType;
    public Expression Expression => _queryable.Expression;
    public IQueryProvider Provider => _queryable.Provider;

    public IQueryable CreateQuery(Expression expression)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        throw new NotImplementedException();
    }

    public object? Execute(Expression expression)
    {
        throw new NotImplementedException();
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _queryable.Provider.Execute<TResult>(expression);
    }

    public IEnumerator<TState> GetEnumerator()
    {
        return _queryable.GetEnumerator();
    }

    public async IAsyncEnumerator<TState> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
    {
        var cursor = await _queryable.ToCursorAsync(cancellationToken);

        while (await cursor.MoveNextAsync(cancellationToken))
        {
            foreach (var item in cursor.Current)
            {
                yield return item;
            }
        }
    }

    public async ValueTask<bool> AnyAsync()
    {
        return await _queryable.AnyAsync();
    }

    public async ValueTask<int> CountAsync()
    {
        return await _queryable.CountAsync();
    }

    public IQuery<TState> Where(Expression<Func<TState, bool>> predicate)
    {
        return new MongoQuery<TState>(_queryable.Where(predicate));
    }

    public IQuery<TState> Skip(int count)
    {
        return new MongoQuery<TState>(_queryable.Skip(count));
    }

    public IQuery<TState> Take(int count)
    {
        return new MongoQuery<TState>(_queryable.Take(count));
    }
}