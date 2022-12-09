using Featurize.Repositories.YamlFileRepository;

namespace Featurize.Repositories;

public static class YamlRepositoryOptions
{
    public static RepositoryOptions Directory(this RepositoryOptions options, string value)
    {
        options.Add("Directory", value);
        return options;
    }

    public static RepositoryProviderOptions AddYamlRepository<TEntity>(this RepositoryProviderOptions options, string directory)
        where TEntity : class, IIdentifiable<TEntity, YamlFilename>
    {
        if (!options.Providers.OfType<YamlFileRepositoryProvider>().Any())
        {
            options.AddProvider(new YamlFileRepositoryProvider());
        }

        options.AddRepository<TEntity, YamlFilename>(
            options => options.Directory(directory).Provider("Yaml"));

        return options;
    }
}
