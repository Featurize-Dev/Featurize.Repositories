using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.FileRepository.Tests;

public class RepositoryProvider_Tests
{
    public class Ctor
    {
        [Test]
        public void Should_Set_values()
        {
            var serialier = YamlWrapper.Create();
            var provider = new FileRepositoryProvider(serialier);

            provider.Serializer.Should().NotBeNull();
            provider.Serializer.Should().BeOfType<YamlWrapper>();
            provider.Name.Should().Be(serialier.ProviderName);
        }
    }

    public class ConfigureProvider
    {
        [Test]
        public void should_register_IFileSerializer()
        {
            var services = new ServiceCollection();
            var serialier = YamlWrapper.Create();
            var provider = new FileRepositoryProvider(serialier);

            provider.ConfigureProvider(services);

            var result = services.BuildServiceProvider();

            var serializer = result.GetRequiredService<IFileSerializer>();

            serializer.Should().NotBeNull();
            serializer.Should().BeOfType<YamlWrapper>();
        }
    }
}
