namespace Featurize.Repositories;

/// <summary>
/// Generic options collection
/// </summary>
public class RepositoryOptions : Dictionary<string, string>
{
    /// <summary>
    /// Sets the name of the <see cref="IRepositoryProvider" /> registerd in the <see cref="ProviderCollection"/>
    /// </summary>
    /// <param name="name">Name if the provider</param>
    public void Provider(string name)
    {
        this[nameof(Provider)] = name;
    }

    /// <summary>
    /// Gets the name of the provider to use to create this repository.
    /// </summary>
    /// <returns></returns>
    public string GetProviderName()
    {
        if(TryGetValue(nameof(Provider), out string? value)) {
            return value;
        }

        return string.Empty;
    }
}