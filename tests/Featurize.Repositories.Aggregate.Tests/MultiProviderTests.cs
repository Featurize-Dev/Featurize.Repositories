using Featurize.Repositories.Aggregates;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Moq;

namespace Featurize.Repositories.Aggregate.Tests;
public class MultiProviderTests
{
    
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
        var providerMock = new Mock<IRepositoryProvider>();
        var providerMock1 = new Mock<IRepositoryProvider>();
        var called1 = 0;
        var called2 = 0;

        providerMock.SetupGet(x => x.Name).Returns("Test");
        providerMock.SetupGet(x => x.IsConfigured).Returns(true);
        providerMock.Setup(x => x.ConfigureRepository(It.IsAny<IServiceCollection>(), It.IsAny<RepositoryInfo>()))
            .Callback(() => called1++)
            .Verifiable();

        providerMock1.SetupGet(x => x.Name).Returns("Test1");
        providerMock1.SetupGet(x => x.IsConfigured).Returns(true);
        providerMock1.Setup(x => x.ConfigureRepository(It.IsAny<IServiceCollection>(), It.IsAny<RepositoryInfo>()))
            .Callback(() => called2++)
            .Verifiable();

        features.AddRepositories(x => {
            x.AddProvider(providerMock.Object);
            x.AddProvider(providerMock1.Object);
            x.AddAggregate<MongoAggregate, Guid>(x =>
            {
                x.Provider("Test");
            });
            x.AddAggregate<MemmoryAggregate, Guid>(x =>
            {
                x.Provider("Test1");
            });
        });

        foreach (var f in features.OfType<IServiceCollectionFeature>())
        {
            f.Configure(services);
        }

        var provider = services.BuildServiceProvider();

        providerMock.Verify();
        providerMock1.Verify();
        called1.Should().Be(1);
        called2.Should().Be(1);

    }

}

public class TestProvider : IRepositoryProvider
{
    public TestProvider(string name)
    {
        Name = name;
    }
    public string Name { get; private set; }

    public bool IsConfigured => true;

    public void ConfigureProvider(IServiceCollection services)
    {
        
    }

    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        
    }
}


