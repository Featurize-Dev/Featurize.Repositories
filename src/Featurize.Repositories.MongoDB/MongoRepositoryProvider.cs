using Featurize.Repositories.DefaultProvider;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Runtime.CompilerServices;

namespace Featurize.Repositories.MongoDB;
public class MongoRepositoryProvider : IRepositoryProvider
{
	public MongoRepositoryProvider(string connectionString)
	{
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }

    public void ConfigureProvider(IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(x => new MongoClient(ConnectionString));
    }

    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var collectionType = typeof(IMongoCollection<>).MakeGenericType(info.EntityType);
        
        services.AddTransient(collectionType, c =>
        {
            var client = c.GetRequiredService<IMongoClient>();
            var database = client.GetDatabase(info.Options.GetDatabase());
            var type = database.GetType();
            var method = type.GetMethod(nameof(database.GetCollection))?.MakeGenericMethod(info.EntityType);
            return method?.Invoke(database, new[] { info.Options.GetCollectionName(), null })!;
        });



        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        var implType = typeof(MongoEntityRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        services.AddTransient(serviceType, implType);
    }
}
