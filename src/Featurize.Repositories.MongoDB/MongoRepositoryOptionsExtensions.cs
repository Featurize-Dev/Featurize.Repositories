namespace Featurize.Repositories.MongoDB;

public static class MongoRepositoryOptionsExtensions
{
    public static RepositoryProviderOptions AddMongo(this RepositoryProviderOptions options, string connectionString)
        => options.AddMongo(connectionString, MongoRepositoryProvider.DefaultName);

    public static RepositoryProviderOptions AddMongo(this RepositoryProviderOptions options, string connectionString, string name)
    {
        options.Providers.Add(new MongoRepositoryProvider(connectionString, name));
        return options;
    }


    public static RepositoryOptions Database(this RepositoryOptions options, string value)
    {
        options[nameof(Database)] = value;
        return options;
    }
    public static RepositoryOptions CollectionName(this RepositoryOptions options, string value)
    {
        options[nameof(CollectionName)]  = value;
        return options;
    }

    public static string GetDatabase(this RepositoryOptions options)
    {
        if (options.TryGetValue(nameof(Database), out string? value))
        {
            return value;
        }
        return string.Empty;
    }
    public static string GetCollectionName(this RepositoryOptions options)
    {
        if (options.TryGetValue(nameof(CollectionName), out string? value))
        {
            return value;
        }
        return string.Empty;
    }
}
