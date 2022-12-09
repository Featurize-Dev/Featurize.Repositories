using Featurize.Repositories.FileRepository;

namespace Featurize.Repositories;

/// <summary>
/// Repository Proviver Options extensions.
/// </summary>
public static class RepositoryProviderOptionsExtensions
{
    /// <summary>
    /// Adds the file repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="options">The options.</param>
    /// <param name="directory">The directory.</param>
    /// <param name="provider">The provider.</param>
    /// <returns></returns>
    public static RepositoryProviderOptions AddFileRepository<TEntity>(this RepositoryProviderOptions options, string directory, string provider)
        where TEntity : class, IIdentifiable<TEntity, Filename>
    {
        options.AddRepository<TEntity, Filename>(options => options.Directory(directory).Provider(provider));
        return options;
    }
}
