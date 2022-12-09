using Featurize.Repositories;

namespace Featurize.Repositories.YamlFileRepository;

public interface IYamlFileRepository<TEntity> : IEntityRepository<TEntity, YamlFilename>
    where TEntity : class, IIdentifiable<TEntity, YamlFilename>, new()
{
}
