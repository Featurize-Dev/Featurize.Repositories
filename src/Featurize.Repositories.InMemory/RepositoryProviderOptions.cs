namespace Featurize.Repositories.InMemory;
/// <summary>
/// 
/// </summary>
public static class RepositoryProviderOptionsExtentions
{
    /// <summary>
    /// Uses the in memory.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns></returns>
    public static RepositoryProviderOptions AddInMemory(this RepositoryProviderOptions options)
    {
        options.AddProvider(new InMemoryRepositoryProvider());
        return options;
    }

}
