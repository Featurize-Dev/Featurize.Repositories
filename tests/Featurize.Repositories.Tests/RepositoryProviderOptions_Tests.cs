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


            options.Providers.Count().Should().Be(1);
            options.Providers.Should().BeOfType<ProviderCollection>();
            options.Providers.First().Should().BeOfType<DefaultRepositoryProvider>();

            options.Repositories.Should().NotBeNull();
            options.Repositories.Should().BeOfType<RepositoryCollection>();
            options.Repositories.Count().Should().Be(0);
        }
    }

    public class AddRepository
    {
        public void calls_add_on_repository_collection()
        {

        }
    }
}

public class TestRepositoryCollection : IRepositoryCollection
{
    private readonly List<RepositoryInfo> _repositoryInfos = new();

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
