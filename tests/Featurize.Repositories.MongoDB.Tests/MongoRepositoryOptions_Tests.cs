using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories.MongoDB.Tests;
public class MongoRepositoryOptions_Tests
{
    public class GetCollectionName
    {
        [Test]
        public void returns_collectionName()
        {
            var options = new RepositoryOptions();
            var value = Guid.NewGuid().ToString();
            options.CollectionName(value);

            var result = options.GetCollectionName();

            result.Should().Be(value);
        }
    }

    public class GetDatabase
    {
        [Test]
        public void returns_collectionName()
        {
            var options = new RepositoryOptions();
            var value = Guid.NewGuid().ToString();
            options.Database(value);

            var result = options.GetDatabase();

            result.Should().Be(value);
        }
    }

    public class CollectionName
    {
        [Test]
        public void Should_insert_repository_options()
        {
            var options = new RepositoryOptions();
            var value = Guid.NewGuid().ToString();
            options.CollectionName(value);

            var item = options.FirstOrDefault();

            item.Key.Should().Be("CollectionName");
            item.Value.Should().Be(value);
        }
    }

    public class Database
    {
        [Test]
        public void Should_insert_repository_options()
        {
            var options = new RepositoryOptions();
            var value = Guid.NewGuid().ToString();
            options.Database(value);

            var item = options.FirstOrDefault();

            item.Key.Should().Be("Database");
            item.Value.Should().Be(value);
        }
    }
}
