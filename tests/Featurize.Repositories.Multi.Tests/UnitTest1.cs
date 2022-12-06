using Featurize.Repositories.InMemory;
using Featurize.Repositories.MongoDB;
using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.Multi.Tests;

public class Tests
{

    [Test]
    public async Task Test1()
    {
        var services = new ServiceCollection();
        var featureCollection = new FeatureCollection();

        featureCollection.AddRepositoryProvider(options =>
        {
            options.UseMongo("mongodb://username:password@localhost:27017");
            options.AddRepository<MongoEntity, Guid>(o => {
                o.Database("Test");
                o.CollectionName("TestCollection");
            });
        });

        featureCollection.AddRepositoryProvider(options =>
        {
            options.UseInMemory();
            options.AddRepository<MemoryEntity, Guid>();
        });

        foreach (var feature in featureCollection.GetServiceCollectionFeatures())
        {
            feature.Configure(services);
        }

        var provider = services.BuildServiceProvider();

        var repository = provider.GetRequiredService<IEntityRepository<MongoEntity, Guid>>();
        var repository1 = provider.GetRequiredService<IEntityRepository<MemoryEntity, Guid>>();

        var allItems = await repository.Query.CountAsync();
                
        Assert.Pass();
    }
}

public class MemoryEntity : IIdentifiable<MemoryEntity, Guid>
{
    public Guid Id { get; set; }

    public static Guid Identify(MemoryEntity entity)
    {
        return entity.Id;
    }
}

public class MongoEntity : IIdentifiable<MongoEntity, Guid>
{
    public Guid Id { get; set; }

    public static Guid Identify(MongoEntity entity)
    {
        return entity.Id;
    }
}