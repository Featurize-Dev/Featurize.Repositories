using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories;
public interface IQueryableRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class
    where TId : struct
{
    IQuery<TEntity> Query { get; }
}

public interface IQuery<TEntity> : IAsyncEnumerable<TEntity>
{
    ValueTask<bool> AnyAsync();

    ValueTask<int> CountAsync();

    IQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

    IQuery<TEntity> Skip(int count);

    IQuery<TEntity> Take(int count);
}