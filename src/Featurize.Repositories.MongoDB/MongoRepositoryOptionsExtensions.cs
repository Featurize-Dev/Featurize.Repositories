namespace Featurize.Repositories.MongoDB;

public static class MongoRepositoryOptionsExtensions
{

    public static RepositoryOptions Database(this RepositoryOptions options, string value)
    {
        options.Set(nameof(Database), value);
        return options;
    }
    public static RepositoryOptions CollectionName(this RepositoryOptions options, string value)
    {
        options.Set(nameof(CollectionName), value);
        return options;
    }

    internal static string GetDatabase(this RepositoryOptions options)
        => options.Get(nameof(Database));
    internal static string GetCollectionName(this RepositoryOptions options)
        => options.Get(nameof(CollectionName));
}
