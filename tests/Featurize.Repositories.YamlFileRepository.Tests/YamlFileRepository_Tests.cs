using Featurize.Repositories;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Featurize.Repositories.YamlFileRepository.Tests;

public class YamlFileRepository_Tests
{
    public static ISerializer Serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeInspector(x => new SortedTypeInspector(x))
            .ConfigureDefaultValuesHandling(
                DefaultValuesHandling.OmitNull |
                DefaultValuesHandling.OmitDefaults |
                DefaultValuesHandling.OmitEmptyCollections)
            .Build();

    public static IDeserializer Deserializer = new DeserializerBuilder()
               .WithNamingConvention(CamelCaseNamingConvention.Instance)
               .WithTypeInspector(x => new SortedTypeInspector(x))
               .Build();


    public class Ctor
    {
        public void should_throw_if_arguments_null()
        {
            Assert.Throws<ArgumentNullException>(() => new YamlFileRepository<TestEntity>(null, null, null));
            Assert.Throws<ArgumentNullException>(() => new YamlFileRepository<TestEntity>(Serializer, null, null));
            Assert.Throws<ArgumentNullException>(() => new YamlFileRepository<TestEntity>(Serializer, Deserializer, null));
        }

        public void should_set_values()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var repo = new YamlFileRepository<TestEntity>(Serializer, Deserializer, directory);

            Assert.AreEqual(Serializer, repo.Serializer);
            Assert.AreEqual(Deserializer, repo.Deserializer);
            Assert.AreEqual(Path.Combine(directory, typeof(TestEntity).Name), repo.BaseDirectory);
            Assert.AreEqual(Path.Combine(directory, typeof(TestEntity).Name), repo.BaseDirectory, "deleted");
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
            var repo = new YamlFileRepository<TestEntity>(Serializer, Deserializer, directory);
            var filename = YamlFilename.Create(Guid.NewGuid().ToString());
            var baseDirectory = Path.Combine(directory, typeof(TestEntity).Name);
            var file = Path.Combine(baseDirectory, filename.ToString());
            var deletedFile = Path.Combine(baseDirectory, "deleted", filename.ToString());

            File.WriteAllBytes(file, Array.Empty<byte>());

            Assert.True(File.Exists(file));

            await repo.DeleteAsync(filename);

            Assert.False(File.Exists(file));
            Assert.True(File.Exists(deletedFile));
        }
    }

    
    public class FindByIdAsync
    {
        [Test]
        public async Task should_return_null_if_not_exists()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var repo = new YamlFileRepository<TestEntity>(Serializer, Deserializer, directory);

            var entity = new TestEntity() { Id = Guid.NewGuid() };

            var result = await repo.FindByIdAsync(YamlFilename.Create(entity.Id.ToString()));

            Assert.IsNull(result);
        }

        [Test]
        public async Task should_return_item_if_exists()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var repo = new YamlFileRepository<TestEntity>(Serializer, Deserializer, directory);

            var entity = new TestEntity() { Id = Guid.NewGuid() };

            await repo.SaveAsync(entity);

            var result = await repo.FindByIdAsync(YamlFilename.Create(entity.Id.ToString()));

            Assert.IsNotNull(result);
        }
    }

    public class SaveAsync
    {
        [Test]
        public async Task should_create_file()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var repo = new YamlFileRepository<TestEntity>(Serializer, Deserializer, directory);
            var entity = new TestEntity() { Id = Guid.NewGuid() };

            var filename = YamlFilename.Create(entity.Id.ToString());
            var file = Path.Combine(directory, entity.GetType().Name, filename.ToString());

            await repo.SaveAsync(entity);

            Assert.True(File.Exists(file));
        }
    }
}

public class YamlFilename_Tests
{

}

public class YamlRepositoryProvider_Tests
{

}

public class YamlRepositoryOptions_Tests
{

}

public class TestEntity : IIdentifiable<TestEntity, YamlFilename>
{
    public Guid Id { get; set; }
    public static YamlFilename Identify(TestEntity entity)
    {
        return YamlFilename.Create(entity.Id.ToString());
    }
}