using EphemeralMongo;
using MongoDB.Driver;

namespace Featurize.Repositories.MongoDB.Tests;
internal class MongoRepository_Tests
{
    private IMongoRunner _runner;

    [SetUp]
    public void Setup()
    {
        _runner = MongoRunner.Run();
    }

    private MongoEntityRepository<Entity, Guid> CreateRepo(string collectionName)
    {

        var client = new MongoClient(_runner.ConnectionString);
        var database = client.GetDatabase("Test");
        var collection = database.GetCollection<Entity>(collectionName);
        return new MongoEntityRepository<Entity, Guid>(collection);
    }

   
    [Test]
    public async Task SaveAsync_should_insert_entity_in_collection()
    {
        var repo = CreateRepo(Guid.NewGuid().ToString());

        var entity = new Entity() { Id = Guid.NewGuid() };

        await repo.SaveAsync(entity);

        repo.Collection.AsQueryable().Should().HaveCount(1);

    }

    [Test]
    public async Task DeleteAsync_should_delete_entity_in_collection()
    {
        var repo = CreateRepo(Guid.NewGuid().ToString());

        var entity = new Entity() { Id = Guid.NewGuid() };

        await repo.SaveAsync(entity);

        repo.Collection.AsQueryable().Should().HaveCount(1);

        await repo.DeleteAsync(entity.Id);

        repo.Collection.AsQueryable().Should().HaveCount(0);
    }

    [Test]
    public async Task FindByIdAsync_should_find_entity_in_collection()
    {
        var repo = CreateRepo(Guid.NewGuid().ToString());

        var entity = new Entity() { Id = Guid.NewGuid() };

        await repo.SaveAsync(entity);

        repo.Collection.AsQueryable().Should().HaveCount(1);

        var result = await repo.FindByIdAsync(entity.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
    }



    [TearDown]
    public void TearDown()
    {
        _runner.Dispose();
    }
}
