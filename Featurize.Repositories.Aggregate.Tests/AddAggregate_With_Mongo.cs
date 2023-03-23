using EphemeralMongo;
using Featurize.Repositories.Aggregates;
using Featurize.Repositories.MongoDB;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Featurize.Repositories.Aggregate.Tests;

public class AddAggregate_With_Mongo
{
    private IMongoRunner _runner;

    [SetUp]
    public void Setup()
    {
        _runner = MongoRunner.Run();
    }

    [Test]
    public async Task Should_register_EventRepsitory()
    {
        var services = new ServiceCollection();
        var features = new FeatureCollection();


        // required. driver will block IEvent because considered unsafe.
        var objectSerializer = new ObjectSerializer(type => ObjectSerializer.DefaultAllowedTypes(type) || type.IsAssignableTo(typeof(IEvent)));
        BsonSerializer.RegisterSerializer(objectSerializer);

        features.AddRepositories(x => {
            x.AddMongo(_runner.ConnectionString);
            //x.AddMongo("mongodb://username:password@localhost:27017");
            x.AddAggregate<TestAggregate, Guid>(x =>
            {
                x.Provider("MongoDB");
                x.Database("Test");
                x.CollectionName("Test");
            });
        });

        foreach(var f in features.OfType<IServiceCollectionFeature>())
        {
            f.Configure(services);
        }

        var provider = services.BuildServiceProvider();

        var repo = provider.GetRequiredService<IRepository<TestAggregate, Guid>>();
        
        var aggregateId = Guid.NewGuid();

        var a = TestAggregate.Create(aggregateId);

        a.ChangeName("Test");
        a.ChangeName("Test1");
        a.ChangeName("Test2");

        await repo.SaveAsync(a);

        var b = await repo.FindByIdAsync(aggregateId);

        b.Should().NotBeNull();
        b?.Version.Should().Be(3);
        b?.Name.Should().Be("Test2");
        b?.Id.Should().Be(aggregateId);
    }

    [TearDown]
    public void TearDown()
    {
        _runner.Dispose();
    }
}


