using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.Tests;

public class AddRepositoryProvider
{
    [Test]
    public void should_add_to_features()
    {
        var features = new FeatureCollection();

        features.AddRepositories(options =>
        {
        });

        features.Count.Should().Be(1);
    }

    [Test]
    public void with_added_repository_should_register_repository()
    {
        var features = new FeatureCollection();
        var serviceCollection = new ServiceCollection();

        features.AddRepositories(options =>
        {
            options.AddProvider(new TestRepositoryProvider());
            options.AddRepository<TestEntity, Guid>();
        });

        features.Count.Should().Be(1);

        foreach(var feature in features.GetServiceCollectionFeatures())
        {
            feature.Configure(serviceCollection);
        }

        var provider = serviceCollection.BuildServiceProvider();
    }

    [Test]
    public void with_added_with_named_repository_should_register_repository()
    {
        var features = new FeatureCollection();
        var serviceCollection = new ServiceCollection();
        var providerName = Guid.NewGuid().ToString();

        features.AddRepositories(options =>
        {
            options.AddProvider(new TestRepositoryProvider());
            options.AddProvider(new TestRepositoryProvider(providerName));
            options.AddRepository<TestEntity, Guid>();
            options.AddRepository<TestEntity2, Guid>(x => x.Provider(providerName));
        });

        features.Count.Should().Be(1);

        foreach (var feature in features.GetServiceCollectionFeatures())
        {
            feature.Configure(serviceCollection);
        }

        var provider = serviceCollection.BuildServiceProvider();


    }
}
