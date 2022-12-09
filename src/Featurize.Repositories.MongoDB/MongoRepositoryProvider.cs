using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Featurize.Repositories.MongoDB;
/// <summary>
/// 
/// </summary>
/// <seealso cref="Featurize.Repositories.IRepositoryProvider" />
public sealed class MongoRepositoryProvider : IRepositoryProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoRepositoryProvider"/> class.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    public MongoRepositoryProvider(string connectionString) 
        : this(connectionString, DefaultName) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoRepositoryProvider"/> class.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="name">The name.</param>
    public MongoRepositoryProvider(string connectionString, string name)
	{
        Name = name;
        ConnectionString = connectionString;
    }
    /// <summary>
    /// Gets the default name.
    /// </summary>
    /// <value>
    /// The default name.
    /// </value>
    public static string DefaultName => "MongoDB";

    /// <summary>
    /// Gets the connection string.
    /// </summary>
    /// <value>
    /// The connection string.
    /// </value>
    public string ConnectionString { get; }
    /// <summary>
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// </summary>
    public bool IsConfigured { get; }

    /// <summary>
    /// Configures services required for this provider.
    /// </summary>
    /// <param name="services">The service collection <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
    public void ConfigureProvider(IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(x => new MongoClient(ConnectionString));
    }
    /// <summary>
    /// Configures the services for the repository.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="info"></param>
    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var collectionType = typeof(IMongoCollection<>).MakeGenericType(info.EntityType);
        
        services.AddTransient(collectionType, c =>
        {
            var client = c.GetRequiredService<IMongoClient>();
            var database = client.GetDatabase(GetDatabase(info.Options));
            var type = database.GetType();
            var method = type.GetMethod(nameof(database.GetCollection))?.MakeGenericMethod(info.EntityType);
            return method?.Invoke(database, new[] { GetCollectionName(info.Options), null })!;
        });



        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        var implType = typeof(MongoEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        services.AddTransient(serviceType, implType);
    }

    private static string GetDatabase(RepositoryOptions options)
    {
        if (options.TryGetValue("Database", out string? value))
        {
            return value;
        }
        return string.Empty;
    }
    private static string GetCollectionName(RepositoryOptions options)
    {
        if (options.TryGetValue("CollectionName", out string? value))
        {
            return value;
        }
        return string.Empty;
    }
}
