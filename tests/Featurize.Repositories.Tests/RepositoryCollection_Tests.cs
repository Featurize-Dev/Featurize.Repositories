using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Featurize.Repositories.Tests;
public class RepositoryCollection_Tests
{
    public class Add
    {
        [Test]
        public void should_increase_count()
        {
            var collection = new RepositoryCollection();

            collection.Add(new RepositoryInfo(typeof(TestEntity), typeof(Guid), new RepositoryOptions()));

            collection.Should().HaveCount(1);
            collection.Count().Should().Be(1);
        }
    }
}
