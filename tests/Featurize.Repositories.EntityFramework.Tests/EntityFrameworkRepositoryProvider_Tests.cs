using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.EntityFramework.Tests;

public sealed class EntityFrameworkRepositoryProvider_Tests
{
    public class Ctor
    {
        [Test]
        public void should_set_options()
        {
            var options = new EntityFrameworkRepositoryProviderOptions();
            var provider = new EntityFrameworkRepositoryProvider(options);

            provider.Options.Should().Be(options);
        }
    }

    public class ConfigureProvider
    {
        [Test]
        public void should_register_Services()
        {
            var services = new ServiceCollection();

            var provider = new EntityFrameworkRepositoryProvider(new EntityFrameworkRepositoryProviderOptions());

            provider.ConfigureProvider(services);

            var sp = services.BuildServiceProvider();

            // TODO: Check if Services are registerd
        }
    }

    public class ConfigureRepository
    {
        [Test]
        public void should_register_repository()
        {
            var services = new ServiceCollection();
            
            services.AddDbContext<TestContext>(x => x.UseInMemoryDatabase("test"));
            
            var provider = new EntityFrameworkRepositoryProvider(new EntityFrameworkRepositoryProviderOptions());

            var options = new RepositoryOptions()
                .UseContext<TestContext>();

            var repoInfo = new RepositoryInfo(typeof(TestEntity), typeof(Guid), options);

            provider.ConfigureProvider(services);
            provider.ConfigureRepository(services, repoInfo);

            var sp = services.BuildServiceProvider();

            var repo = sp.GetService<IEntityRepository<TestEntity, Guid>>();
            var baseRepo = sp.GetService<IRepository<TestEntity, Guid>>();

            baseRepo.Should().NotBeNull();
            repo.Should().NotBeNull();
        }
    }
}
