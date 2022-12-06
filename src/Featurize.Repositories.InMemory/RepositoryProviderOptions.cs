namespace Featurize.Repositories.InMemory;
public static class RepositoryProviderOptionsExtentions
{
    public static RepositoryProviderOptions UseInMemory(this RepositoryProviderOptions options)
    {
        options.Provider = new InMemoryRepositoryProvider();
        return options;
    }

}
