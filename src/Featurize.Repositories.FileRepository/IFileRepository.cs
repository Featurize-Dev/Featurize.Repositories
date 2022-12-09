namespace Featurize.Repositories.FileRepository;

/// <summary>
/// Defines a type for a FileRepository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IFileRepository<TEntity> : IEntityRepository<TEntity, Filename>
    where TEntity : class, IIdentifiable<TEntity, Filename>, new()
{
}
