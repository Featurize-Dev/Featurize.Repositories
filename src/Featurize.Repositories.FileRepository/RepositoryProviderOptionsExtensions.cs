using Featurize.Repositories.FileRepository;

namespace Featurize.Repositories;
public static class RepositoryProviderOptionsExtensions
{
    public static RepositoryProviderOptions AddFileRepository<TEntity>(this RepositoryProviderOptions options, string directory, string provider)
        where TEntity : class, IIdentifiable<TEntity, Filename>
    {
        options.AddRepository<TEntity, Filename>(options => options.Directory(directory).Provider(provider));
        return options;
    }
}
