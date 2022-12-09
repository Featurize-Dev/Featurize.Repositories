using FluentAssertions;

namespace Featurize.Repositories.FileRepository.Tests;

public class RepositoryOptions_Tests
{
    public class Directory
    {
        [Test]
        public void should_add_directory_option()
        {
            var options = new RepositoryOptions();

            options.Directory("Entity");

            options.Should().HaveCount(1);
            options.Should().ContainKey("Directory");
            options.Should().ContainValue("Entity");
        }
    }
}
