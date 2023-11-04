using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.EntityFramework.Tests;

internal class EntityFrameworkRepository_Tests
{
    private IServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        var provider = new EntityFrameworkRepositoryProvider(new EntityFrameworkRepositoryProviderOptions());

        services.AddDbContext<TestContext>(x => x.UseInMemoryDatabase("test"), contextLifetime: ServiceLifetime.Transient);
        
        provider.ConfigureProvider(services);
        provider.ConfigureRepository(services, new RepositoryInfo(typeof(TestEntity), typeof(Guid), new RepositoryOptions().UseContext<TestContext>()));

        _serviceProvider = services.BuildServiceProvider();
    }

    [Test]
    public async Task SaveAsync_should_insert_entity_in_database()
    {
        var repo = _serviceProvider.GetRequiredService<IRepository<TestEntity, Guid>>();
        var context = _serviceProvider.GetRequiredService<TestContext>();
        var count = context.Entities.Count();

        var entity = new TestEntity {  Id = Guid.NewGuid() };

        await repo.SaveAsync(entity);

        context.Entities.Should().HaveCount(count + 1);
    }

    [Test]
    public async Task DeleteAsync_should_delete_entity_in_Database()
    {
        var repo = _serviceProvider.GetRequiredService<IRepository<TestEntity, Guid>>();
        var context = _serviceProvider.GetRequiredService<TestContext>();
        var count = context.Entities.Count();

        var entity = new TestEntity() { Id = Guid.NewGuid() };

        await repo.SaveAsync(entity);

        context.Entities.Should().HaveCount(count + 1);

        await repo.DeleteAsync(entity.Id);

        context.Entities.Should().HaveCount(count);
    }

    [Test]
    public async Task FindByIdAsync_should_find_entity_in_database()
    {
        var repo = _serviceProvider.GetRequiredService<IRepository<TestEntity, Guid>>();
        var context = _serviceProvider.GetRequiredService<TestContext>();
        var count = context.Entities.Count();

        var entity = new TestEntity() { Id = Guid.NewGuid() };
        await repo.SaveAsync(entity);

        context.Entities.Should().HaveCount(count + 1);

        var result = await repo.FindByIdAsync(entity.Id);

        result.Should().NotBeNull();
        result?.Id.Should().Be(entity.Id);
    }
}
