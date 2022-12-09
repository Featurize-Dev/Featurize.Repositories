namespace Featurize.Repositories.FileRepository;

public interface IFileRepository<TEntity> : IEntityRepository<TEntity, Filename>
    where TEntity : class, IIdentifiable<TEntity, Filename>, new()
{
}
