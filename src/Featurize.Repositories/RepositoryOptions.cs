using System.Collections;

namespace Featurize.Repositories;

/// <summary>
/// Generic options collection
/// </summary>
public sealed class RepositoryOptions : IEnumerable<KeyValuePair<string, string>>
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

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() 
        => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}