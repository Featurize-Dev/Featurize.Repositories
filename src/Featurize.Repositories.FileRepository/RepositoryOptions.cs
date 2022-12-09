namespace Featurize.Repositories.FileRepository;

public static class RepositoryOptionsExtentions
{
    public static RepositoryOptions Directory(this RepositoryOptions options, string value)
    {
        options.Add("Directory", value);
        return options;
    }

    public static RepositoryProviderOptions UseFile(this RepositoryProviderOptions options, IFileSerializer serializer, string providerName)
    {
        options.AddProvider(new FileRepositoryProvider(serializer, providerName));
        return options;
    }
}
