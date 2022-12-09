using Featurize.Repositories;
using Featurize.Repositories.FileRepository;

namespace Featurize.Repositories.FileRepository;

public static class RepositoryOptionsExtentions
{
    public static RepositoryOptions Directory(this RepositoryOptions options, string value)
    {
        options.Add("Directory", value);
        return options;
    }

    public static RepositoryProviderOptions AddYamlRepository<TEntity>(this RepositoryProviderOptions options, string directory)
        where TEntity : class, IIdentifiable<TEntity, Filename>
    {
        if (!options.Providers.OfType<FileRepositoryProvider>().Any())
        {
            options.AddProvider(new FileRepositoryProvider());
        }

        options.AddRepository<TEntity, Filename>(
            options => options.Directory(directory).Provider("Yaml"));

        return options;
    }
}
