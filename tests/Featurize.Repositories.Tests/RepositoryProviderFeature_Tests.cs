using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.Tests;
public class RepositoryProviderFeature_Tests
{
    public class Create
    {
        [Test]
        public void Options_should_be_set()
        {
            var options = new RepositoryProviderOptions();
            var feature = RepositoryProviderFeature.Create(options);
            
            feature.Options.Should().Be(options);
            feature.Name.Should().Be(options.Provider.GetType().Name);
        }

        [Test]
        public void Name_should_be_provider_name()
        {
            var options = new RepositoryProviderOptions
            {
                Provider = new TestRepositoryProvider()
            };

            var feature = RepositoryProviderFeature.Create(options);

            feature.Options.Should().Be(options);
            feature.Name.Should().Be(options.Provider.GetType().Name);
        }
    }

    public class Configure
    {
        [Test]
        public void should_call_provider_configure()
        {
            var serviceCollection = new ServiceCollection();
            var provider = new TestRepositoryProvider();
            var options = new RepositoryProviderOptions
            {
                Provider = provider
            };

            var feature = RepositoryProviderFeature.Create(options);

            feature.Configure(serviceCollection);

            provider.ConfigureProviderCalled.Should().BeTrue();
        }

        [Test]
        public void should_call_ConfigureRepository_when_repository_added()
        {
            var serviceCollection = new ServiceCollection();
            var provider = new TestRepositoryProvider();
            var options = new RepositoryProviderOptions
            {
                Provider = provider,
            };

            options.AddRepository<TestEntity, Guid>();

            var feature = RepositoryProviderFeature.Create(options);

            feature.Configure(serviceCollection);

            provider.ConfigureRepositoryCalled.Should().BeTrue();
        }
    }
}
public class TestRepositoryProvider : IRepositoryProvider
{
    public bool ConfigureProviderCalled { get; set; }
    public bool ConfigureRepositoryCalled { get; set; }

    public void ConfigureProvider(IServiceCollection services)
    {
        ConfigureProviderCalled = true;
    }

    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        ConfigureRepositoryCalled = true;
    }
}
