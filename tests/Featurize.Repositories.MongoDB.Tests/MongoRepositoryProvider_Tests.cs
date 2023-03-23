using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Featurize.Repositories.MongoDB.Tests;

public class MongoRepositoryProvider_Tests
{
    public class Ctor
    {
        [Test]
        public void Set_connectionString()
        {
            var connectionString = Guid.NewGuid().ToString();
            var provider = new MongoRepositoryProvider(connectionString);
            provider.ConnectionString.Should().Be(connectionString);
        }
    }

    public class ConfigureProvider
    {
        [Test]
        public void should_register_MongoClient()
        {
            var services = new ServiceCollection();
            var provider = new MongoRepositoryProvider("mongodb://localhost:27017");
            provider.ConfigureProvider(services);

            services.Should().HaveCount(1);

            var sp = services.BuildServiceProvider();

            var client = sp.GetService<IMongoClient>();

            client.Should().NotBeNull();

        }
    }

    public class ConfigureRepository
    {
        [Test]
        public void should_register_Repository()
        {
            var services = new ServiceCollection();
            var provider = new MongoRepositoryProvider("mongodb://localhost:27017");
            var options = new RepositoryOptions()
                    .Database("database")
                    .CollectionName("collectionName");
            var repoInfo = new RepositoryInfo(typeof(Entity), typeof(Guid), options);
            
            provider.ConfigureProvider(services);
            provider.ConfigureRepository(services, repoInfo);

            var sp = services.BuildServiceProvider();

            var repo = sp.GetService<IEntityRepository<Entity, Guid>>();
            var baseRepo = sp.GetService<IRepository<Entity, Guid>>();

            baseRepo.Should().NotBeNull();
            repo.Should().NotBeNull();
        }
    }
}

public class Entity : IIdentifiable<Entity, Guid>
{
    public Guid Id { get; set; }

    public static Guid Identify(Entity entity)
    {
        return entity.Id;
    }
}