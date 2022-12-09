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
    public static RepositoryProviderOptions UseInMemory(this RepositoryProviderOptions options)
    {
        return options;
    }

}
