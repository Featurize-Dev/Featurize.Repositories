using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories.FileRepository.Tests;
public class IntegrationTests
{
    public class AddFile
    {
        [Test]
        public void should_register_a_repository()
        {
            var features = new FeatureCollection();
            var services = new ServiceCollection();

            features.AddRepositories(options =>
            {
                options.UseFile(YamlWrapper.Create(), "yaml");
                options.AddFileRepository<TestEntity>("", "yaml");
            });

            foreach(var feature in features.GetServiceCollectionFeatures())
            {
                feature.Configure(services);
            }

            var provider = services.BuildServiceProvider();

            var repo = provider.GetRequiredService<IEntityRepository<TestEntity, Filename>>();
            var repo2 = provider.GetRequiredService<IFileRepository<TestEntity>>();

            repo.Should().NotBeNull();
            repo2.Should().NotBeNull();

            repo.Should().BeOfType<FileRepository<TestEntity>>();
            repo2.Should().BeOfType<FileRepository<TestEntity>>();

        }
    }
}
