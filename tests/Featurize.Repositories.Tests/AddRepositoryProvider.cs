using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.Tests;

public class AddRepositoryProvider
{
    [Test]
    public void should_add_to_features()
    {
        var features = new FeatureCollection();

        features.AddRepositoryProvider(options =>
        {
            
        });

        features.Count.Should().Be(1);
    }

    [Test]
    public void with_added_repository_should_register_repository()
    {
        var features = new FeatureCollection();
        var serviceCollection = new ServiceCollection();

        features.AddRepositoryProvider(options =>
        {
            options.AddRepository<TestEntity, Guid>();
        });

        features.Count.Should().Be(1);

        foreach(var feature in features.GetServiceCollectionFeatures())
        {
            feature.Configure(serviceCollection);
        }

        var provider = serviceCollection.BuildServiceProvider();

        var repository = provider.GetService<IRepository<TestEntity, Guid>>();
        var repository1 = provider.GetService<IEntityRepository<TestEntity, Guid>>();
        var repository2 = provider.GetService<IQueryableRepository<TestEntity, Guid>>();

        repository.Should().NotBeNull();
        repository1.Should().NotBeNull();
        repository2.Should().NotBeNull();
    }
}

public class TestEntity : IIdentifiable<TestEntity, Guid>
{
    public Guid Id { get; set; }

    public static Guid Identify(TestEntity entity)
    {
        return entity.Id;
    }
}