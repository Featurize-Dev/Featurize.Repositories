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
        }

        [Test]
        public void Name_should_be_provider_name()
        {
            var options = new RepositoryProviderOptions();
            var feature = RepositoryProviderFeature.Create(options);

            feature.Options.Should().Be(options);
        }
    }

    public class Configure
    {
        [Test]
        public void should_call_ConfigureRepository_when_repository_added()
        {
            var serviceCollection = new ServiceCollection();
            var provider = new TestRepositoryProvider();
            var options = new RepositoryProviderOptions();

            options.AddProvider(provider);
            options.AddRepository<TestEntity, Guid>(o => o.Provider("Test"));

            var feature = RepositoryProviderFeature.Create(options);

            feature.Configure(serviceCollection);

            provider.ConfigureProviderCalled.Should().BeTrue();
            provider.ConfigureRepositoryCalled.Should().BeTrue();
        }
    }
}
public class TestRepositoryProvider : IRepositoryProvider
{
    public static string DefaultName => "Test";

    public bool ConfigureProviderCalled { get; set; }
    public bool ConfigureRepositoryCalled { get; set; }

    public string Name { get; }

    public bool IsConfigured => false;

    public TestRepositoryProvider() : this(TestRepositoryProvider.DefaultName) { }

    public TestRepositoryProvider(string name)
    {
        Name = name;
    }

    public void ConfigureProvider(IServiceCollection services)
    {
        ConfigureProviderCalled = true;
    }

    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        ConfigureRepositoryCalled = true;
    }
}
