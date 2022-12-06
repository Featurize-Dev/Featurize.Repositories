using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories;

/// <summary>
/// Describes an Entity Repository
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IEntityRepository<TEntity, TId> :
    IRepository<TEntity, TId>,
    IQueryableRepository<TEntity, TId>
    
    where TEntity : class
    where TId : struct
{
}
