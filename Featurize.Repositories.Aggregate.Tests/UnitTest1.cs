using Featurize.Repositories.Aggregates;
using Featurize.Repositories.MongoDB;
using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.Aggregate.Tests;

public class AddAggreagete
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Should_register_EventRepsitory()
    {
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        var features = new FeatureCollection();

        features.AddRepositories(x => {
            x.AddMongo("mongodb://username:password@localhost:27017");
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

        await repo.SaveAsync(a);

        var b = repo.FindByIdAsync(aggregateId);


    }
}


public record ChangeNameEvent(string Name) : IEvent;
public class TestAggregate : AggregateRoot<TestAggregate, Guid>,
    IAggregate<TestAggregate, Guid>
{
    private TestAggregate(Guid id) : base(id) { }

    public string Name { get; private set; }

    public static TestAggregate Create(Guid id)
    {
        return new TestAggregate(id);
    }

    public void ChangeName(string name)
    {
        ApplyEvent(new ChangeNameEvent(name));
    }

    public void Apply(ChangeNameEvent e)
    {
        Name = e.Name;
    }

    public static Guid Identify(TestAggregate entity)
    {
        return entity.Id;
    }
}