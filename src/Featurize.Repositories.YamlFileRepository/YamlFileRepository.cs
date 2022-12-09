using Featurize.Repositories;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace Featurize.Repositories.YamlFileRepository;

public class YamlFileRepository<TEntity> : IYamlFileRepository<TEntity>
    where TEntity : class, IIdentifiable<TEntity, YamlFilename>, new()
{
    public YamlFileRepository(ISerializer serializer, IDeserializer deserializer, string path)
    {
        ArgumentNullException.ThrowIfNull(serializer);
        ArgumentNullException.ThrowIfNull(deserializer);
        ArgumentNullException.ThrowIfNull(path);

        Serializer = serializer;
        Deserializer = deserializer;
        BaseDirectory = Path.Combine(path, typeof(TEntity).Name.ToLower());
        DeletedDirectory = Path.Combine(BaseDirectory, "deleted");
    }

    public ISerializer Serializer { get; }
    public IDeserializer Deserializer { get; }
    public string BaseDirectory { get; }
    public string DeletedDirectory { get; }

    public IQuery<TEntity> Query => new DirectoryQueryable<TEntity>(Deserializer, Directory.EnumerateFiles(BaseDirectory));

    public ValueTask DeleteAsync(YamlFilename id)
    {
        ArgumentNullException.ThrowIfNull(id);

        CreateDirectories();

        File.Move(CreateFilename(id), Path.Combine(DeletedDirectory, id.ToString()));
        File.Delete(CreateFilename(id));

        return ValueTask.CompletedTask;
    }

    public async ValueTask<TEntity?> FindByIdAsync(YamlFilename id)
    {
        ArgumentNullException.ThrowIfNull(id);

        CreateDirectories();

        var filename = CreateFilename(id);
        
        if (!File.Exists(filename))
        {
            return default;   
        }

        var result = await File.ReadAllTextAsync(filename);
        var item = Deserializer.Deserialize<TEntity?>(result);

        return item;
    }

    public async ValueTask SaveAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        CreateDirectories();

        var item = Serializer.Serialize(entity);
        var id = TEntity.Identify(entity);

        await File.WriteAllTextAsync(CreateFilename(id), item);
    }

    private string CreateFilename(YamlFilename id)
    {
        return Path.Combine(BaseDirectory, id.ToString());
    }

    private void CreateDirectories()
    {
        Directory.CreateDirectory(BaseDirectory);
        Directory.CreateDirectory(DeletedDirectory);
    }
}
