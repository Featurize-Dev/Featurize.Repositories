namespace Featurize.Repositories.DefaultProvider;

public class DefaultRepositoryProvider : IRepositoryProvider
{
    public Type[] MakeServiceTypes(Type entityType, Type idType)
    {
        return new[]
        {
            typeof(IRepository<,>).MakeGenericType(entityType, idType),
            typeof(IStateRepository<,>).MakeGenericType(entityType, idType),
            typeof(IQueryableRepository<,>).MakeGenericType(entityType, idType),
            typeof(IBufferedRepository<,>).MakeGenericType(entityType, idType),
        };   
    }

    public Type MakeImplementationType(Type wntityType, Type idType)
    {
        return typeof(DefaultRepository<,>).MakeGenericType(wntityType, idType);
    }
}
