using Featurize.Repositories.FileRepository;
using FluentAssertions;
using NUnit.Framework.Constraints;
namespace Featurize.Repositories.FileRepository.Tests;

public class FileRepository_Tests
{
    public class Ctor
    {
        public void should_throw_if_arguments_null()
        {
            Assert.Throws<ArgumentNullException>(() => new FileRepository<TestEntity>(null, null));
            Assert.Throws<ArgumentNullException>(() => new FileRepository<TestEntity>(YamlWrapper.Create(), null));
            Assert.Throws<ArgumentNullException>(() => new FileRepository<TestEntity>(null, ""));
        }

        public void should_set_values()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var repo = new FileRepository<TestEntity>(YamlWrapper.Create(), directory);

            var baseDirectory = Path.Combine(directory, typeof(TestEntity).Name);
            var deletedDirectory = Path.Combine(baseDirectory, "deleted");

            repo.Serializer.Should().Be(YamlWrapper.Create());
            repo.BaseDirectory.Should().Be(baseDirectory);
            repo.DeletedDirectory.Should().Be(deletedDirectory);
        }
    }
    public class Query
    {

    }
    public class DeleteAsync
    {
        [Test]
        public async Task should_remove_file_from_directory()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var repo = new FileRepository<TestEntity>(YamlWrapper.Create(), directory);
            
            var filename = Filename.Create($"{Guid.NewGuid()}.yaml");
           
            var file = Path.Combine(repo.BaseDirectory, filename.ToString());
            var deletedFile = Path.Combine(repo.DeletedDirectory, filename.ToString());

            File.WriteAllBytes(file, Array.Empty<byte>());

            File.Exists(file).Should().BeTrue();

            await repo.DeleteAsync(filename);

            File.Exists(file).Should().BeFalse();
            File.Exists(deletedFile).Should().BeTrue();
        }
    }
    public class FindByIdAsync
    {
        [Test]
        public async Task should_return_null_if_not_exists()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var repo = new FileRepository<TestEntity>(YamlWrapper.Create(), directory);

            var entity = new TestEntity() { Id = Guid.NewGuid() };

            var result = await repo.FindByIdAsync(TestEntity.Identify(entity));

            result.Should().BeNull();
        }

        [Test]
        public async Task should_return_item_if_exists()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var repo = new FileRepository<TestEntity>(YamlWrapper.Create(), directory);

            var entity = new TestEntity() { Id = Guid.NewGuid() };

            await repo.SaveAsync(entity);

            var result = await repo.FindByIdAsync(TestEntity.Identify(entity));

            result.Should().NotBeNull();
            result.Id.Should().Be(entity.Id);
        }
    }
    public class SaveAsync
    {
        [Test]
        public async Task should_create_file()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var repo = new FileRepository<TestEntity>(YamlWrapper.Create(), directory);
            var entity = new TestEntity() { Id = Guid.NewGuid() };

            var filename = TestEntity.Identify(entity);
            var file = Path.Combine(directory, entity.GetType().Name, filename.ToString());

            await repo.SaveAsync(entity);

            File.Exists(file).Should().BeTrue();
        }
    }
}
