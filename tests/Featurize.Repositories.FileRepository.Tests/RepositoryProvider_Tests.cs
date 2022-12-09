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
            var provider = new FileRepositoryProvider(serialier, "yaml");

            provider.Serializer.Should().NotBeNull();
            provider.Serializer.Should().BeOfType<YamlWrapper>();
            provider.Name.Should().Be("yaml");
        }
    }

    public class ConfigureProvider
    {
        [Test]
        public void should_register_IFileSerializer()
        {
            var services = new ServiceCollection();
            var serialier = YamlWrapper.Create();
            var provider = new FileRepositoryProvider(serialier, "yaml");

            provider.ConfigureProvider(services);

            var result = services.BuildServiceProvider();

            var serializer = result.GetRequiredService<IFileSerializer>();

            serializer.Should().NotBeNull();
            serializer.Should().BeOfType<YamlWrapper>();
        }

        [Test]
        public void should_construct_repository()
        {
            var services = new ServiceCollection(); 
            var serialier = YamlWrapper.Create();
            var provider = new FileRepositoryProvider(serialier, "yaml");

            provider.ConfigureProvider(services);
            provider.ConfigureRepository(services, 
                new RepositoryInfo(typeof(TestEntity), 
                                   typeof(Filename), 
                                   new RepositoryOptions().Directory("Test")));

            var result = services.BuildServiceProvider();

            var repo = result.GetRequiredService<IEntityRepository<TestEntity, Filename>>();
            var fileRepo = result.GetRequiredService<IFileRepository<TestEntity>>();

            repo.Should().NotBeNull();
            repo.Should().BeOfType<FileRepository<TestEntity>>();
            fileRepo.Should().NotBeNull();
            fileRepo.Should().BeOfType<FileRepository<TestEntity>>();

        }
    }
}
