namespace Featurize.Repositories;

public interface IRepositoryProvider
{
    public Type MakeImplementationType(Type valueType, Type idType);
    public Type[] MakeServiceTypes(Type entityType, Type idType);
}
