using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
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
    public MongoRepositoryProvider(MongoProviderOptions options)
	{
        Name= options.Name;
        Options = options;
    }
    /// <summary>
    /// Gets the default name.
    /// </summary>
    /// <value>
    /// The default name.
    /// </value>
    public static string DefaultName => "MongoDB";
        
    /// <summary>
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// </summary>
    public bool IsConfigured { get; }

    /// <summary>
    /// Options for this provider instance
    /// </summary>
    public MongoProviderOptions Options { get; }

    /// <summary>
    /// Configures services required for this provider.
    /// </summary>
    /// <param name="services">The service collection <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
    public void ConfigureProvider(IServiceCollection services)
    {
        try
        {
            var objectSerializer = new ObjectSerializer(Options.AllowedTypes);
            BsonSerializer.TryRegisterSerializer(objectSerializer);
        }
        catch { }

        foreach (var provider in Options.SerializationProviders)
                BsonSerializer.RegisterSerializationProvider(provider);

            foreach (var type in Options.ClassMaps)
                BsonClassMap.LookupClassMap(type);
       
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
            var client = new MongoClient(Options.ConnectionString);
            var database = client.GetDatabase(GetDatabase(info.Options));
            var type = database.GetType();
            var method = type.GetMethod(nameof(database.GetCollection))?.MakeGenericMethod(info.EntityType);
            return method?.Invoke(database, new[] { GetCollectionName(info.Options), null })!;
        });

        var repositoryType = typeof(IRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        var implType = typeof(MongoEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        services.AddTransient(serviceType, implType);
        services.AddTransient(repositoryType, implType);
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
