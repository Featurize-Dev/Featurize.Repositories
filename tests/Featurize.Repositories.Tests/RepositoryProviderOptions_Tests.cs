using Featurize.Repositories.DefaultProvider;
using System.Collections;

namespace Featurize.Repositories.Tests;
public class RepositoryProviderOptions_Tests
{
    public class Ctor
    {
        [Test]
        public void Should_have_defaults()
        {
            var options = new RepositoryProviderOptions();

            options.Providers.Should().HaveCount(1);
            options.Providers.First().Should().BeOfType<DefaultRepositoryProvider>();

            options.Repositories.Should().NotBeNull();
            options.Repositories.Should().HaveCount(0);
        }
    }



    public class AddRepository
    {
        [Test]
        public void should_increase_count()
        {
            var options = new RepositoryProviderOptions();

            options.Repositories.Add(new RepositoryInfo(typeof(TestEntity), typeof(Guid), new RepositoryOptions()));

            options.Repositories.Should().HaveCount(1);
        }
    }
}

public class TestRepositoryCollection : IRepositoryCollection
{
    public bool AddCalled { get; set; }

    public int Count => throw new NotImplementedException();

    public void Add(RepositoryInfo item)
    {
        AddCalled = true;
    }

    public IEnumerator<RepositoryInfo> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
