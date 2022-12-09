namespace Featurize.Repositories.FileRepository;

/// <summary>
/// Extension methods for FileRepositoryProvider
/// </summary>
public static class RepositoryOptionsExtentions
{
    
    /// <summary>
    /// Directories the specified value.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static RepositoryOptions Directory(this RepositoryOptions options, string value)
    {
        options.Add("Directory", value);
        return options;
    }

    /// <summary>
    /// Registers the FileRepositoryProvider
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <returns></returns>
    public static RepositoryProviderOptions UseFile(this RepositoryProviderOptions options, IFileSerializer serializer, string providerName)
    {
        options.AddProvider(new FileRepositoryProvider(serializer, providerName));
        return options;
    }
}
