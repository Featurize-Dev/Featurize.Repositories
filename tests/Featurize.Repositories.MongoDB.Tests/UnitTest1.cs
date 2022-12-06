using EphemeralMongo;
using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.MongoDB.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public async Task Test1()
    {
        using var runner = MongoRunner.Run();

        var services = new ServiceCollection();
        var featureCollection = new FeatureCollection();

        featureCollection.AddRepositoryProvider(options =>
        {
            options.UseMongo(runner.ConnectionString);
            options.AddRepository<Entity, Guid>(o =>
            {
                o.Database("Test");
                o.CollectionName("TestCollection");
            });
        });

        foreach (var feature in featureCollection.GetServiceCollectionFeatures())
        {
            feature.Configure(services);
        }

        var provider = services.BuildServiceProvider();

        var repository = provider.GetRequiredService<IEntityRepository<Entity, Guid>>();

        var entity = new Entity()
        {
            Id = Guid.NewGuid(),
        };

        await repository.SaveAsync(entity);

        var entity2 = await repository.FindByIdAsync(entity.Id);

        Assert.Pass();
    }
}

public class Entity : IIdentifiable<Entity, Guid>
{
    public Guid Id { get; set; }

    public static Guid Identify(Entity entity)
    {
        return entity.Id;
    }
}