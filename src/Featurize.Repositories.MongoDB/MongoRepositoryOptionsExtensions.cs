using MongoDB.Driver.Core.Configuration;
using System.Xml.Linq;

namespace Featurize.Repositories.MongoDB;

/// <summary>
/// 
/// </summary>
public static class MongoRepositoryOptionsExtensions
{
    /// <summary>
    /// Adds the mongo.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <returns></returns>
    public static RepositoryProviderOptions AddMongo(this RepositoryProviderOptions options, string connectionString)
        => options.AddMongo(connectionString, MongoRepositoryProvider.DefaultName);

    /// <summary>
    /// Adds the mongo.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static RepositoryProviderOptions AddMongo(this RepositoryProviderOptions options, string connectionString, string name)
        => options.AddMongo(x =>
        {
            x.Name = name;
            x.ConnectionString = connectionString;
        });

    /// <summary>
    /// Adds the MongoDB Repository Provider
    /// </summary>
    /// <param name="o"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static RepositoryProviderOptions AddMongo(this RepositoryProviderOptions o, Action<MongoProviderOptions> options)
    {
        var opt = new MongoProviderOptions();
        options.Invoke(opt);
        o.Providers.Add(new MongoRepositoryProvider(opt));
        return o;
    }

    /// <summary>
    /// Databases the specified value.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static RepositoryOptions Database(this RepositoryOptions options, string value)
    {
        options[nameof(Database)] = value;
        return options;
    }
    /// <summary>
    /// Collections the name.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static RepositoryOptions CollectionName(this RepositoryOptions options, string value)
    {
        options[nameof(CollectionName)]  = value;
        return options;
    }
}
