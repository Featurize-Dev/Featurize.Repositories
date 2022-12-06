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

            options.Repositories.Should().NotBeNull();
            options.Repositories.Should().BeOfType<RepositoryCollection>();
            options.Repositories.Count().Should().Be(0);
            
            options.Provider.Should().NotBeNull();
            options.Provider.Should().BeOfType<DefaultRepositoryProvider>();
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
    private List<RepositoryInfo> _repositoryInfos = new();

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
