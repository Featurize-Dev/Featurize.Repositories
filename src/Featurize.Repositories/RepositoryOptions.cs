namespace Featurize.Repositories;

/// <summary>
/// Generic options collection
/// </summary>
public class RepositoryOptions
{
    private readonly Dictionary<string, string> _items = new();

    /// <summary>
    /// Set additional options for a repository.
    /// </summary>
    /// <param name="key">The key of the setting.</param>
    /// <param name="value">The value of the setting.</param>
    public void Set(string key, string value)
    {
        _items[key] = value;
    }

    /// <summary>
    /// Get additional option for a repository
    /// </summary>
    /// <param name="key"></param>
    /// <returns>Returns the value </returns>
    public string Get(string key) { 
        return _items[key]; 
    }
}