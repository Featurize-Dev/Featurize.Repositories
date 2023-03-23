using Featurize.Repositories.Aggregates;
using Featurize.Repositories.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EphemeralMongo;
using Featurize.Repositories.InMemory;
using FluentAssertions;

namespace Featurize.Repositories.Aggregate.Tests;
public class MultiProviderTests
{
    private IMongoRunner _runner;

    [SetUp]
    public void Setup()
    {
        _runner = MongoRunner.Run();
    }

    public class MongoAggregate : AggregateRoot<MongoAggregate, Guid>, 
        IAggregate<MongoAggregate, Guid>
    {
        private MongoAggregate(Guid id) : base(id)
        {
        }

        public static MongoAggregate Create(Guid id)
        {
            return new MongoAggregate(id);
        }

        public static Guid Identify(MongoAggregate entity)
        {
            return entity.Id;
        }
    }

    public class MemmoryAggregate : AggregateRoot<MemmoryAggregate, Guid>,
        IAggregate<MemmoryAggregate, Guid>
    {
        private MemmoryAggregate(Guid id) : base(id)
        {
        }

        public static MemmoryAggregate Create(Guid id)
        {
            return new MemmoryAggregate(id);
        }

        public static Guid Identify(MemmoryAggregate entity)
        {
            return entity.Id;
        }
    }

    [Test]
    public void should_register_distinct_types()
    {
        var services = new ServiceCollection();
        var features = new FeatureCollection();


        features.AddRepositories(x => {
            x.AddProvider(new InMemoryRepositoryProvider());
            x.AddMongo(_runner.ConnectionString);
            x.AddAggregate<MongoAggregate, Guid>(x =>
            {
                x.Provider(MongoRepositoryProvider.DefaultName);
                x.Database("Test");
                x.CollectionName("Test");
            });
            x.AddAggregate<MemmoryAggregate, Guid>(x =>
            {
                x.Provider(InMemoryRepositoryProvider.DefaultName);
            });
        });

        foreach (var f in features.OfType<IServiceCollectionFeature>())
        {
            f.Configure(services);
        }

        var provider = services.BuildServiceProvider();

        var repo1 = provider.GetRequiredService<IRepository<Event<MongoAggregate, Guid>, Guid>>();
        var repo2 = provider.GetRequiredService<IRepository<Event<MemmoryAggregate, Guid>, Guid>>();

        var baseType1 = repo1.GetType();
        var baseType2 = repo2.GetType();

        baseType1.Should().Be(typeof(MongoEntityRepository<Event<MongoAggregate, Guid>, Guid>));
        baseType2.Should().Be(typeof(InMemoryRepository<Event<MemmoryAggregate, Guid>, Guid>));
    }

    [TearDown]
    public void TearDown()
    {
        _runner.Dispose();
    }
}


