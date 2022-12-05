using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories;
public interface IStateRepository<TEntity, TId> :
    IRepository<TEntity, TId>,
    IBufferedRepository<TEntity, TId>,
    IQueryableRepository<TEntity, TId>
    
    where TEntity : class
    where TId : struct
{
}
