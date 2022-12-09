namespace Featurize.Repositories.FileRepository;

public class FileRepository<TEntity> : IFileRepository<TEntity>
    where TEntity : class, IIdentifiable<TEntity, Filename>, new()
{
    public FileRepository(IFileSerializer serializer, string path)
    {
        ArgumentNullException.ThrowIfNull(serializer);
        ArgumentNullException.ThrowIfNull(path);

        Serializer = serializer;
        BaseDirectory = Path.Combine(path, typeof(TEntity).Name.ToLower());
        DeletedDirectory = Path.Combine(BaseDirectory, "deleted");
    }

    public IFileSerializer Serializer { get; }
    public string BaseDirectory { get; }
    public string DeletedDirectory { get; }

    public IQuery<TEntity> Query => new DirectoryQueryable<TEntity>(Serializer, Directory.EnumerateFiles(BaseDirectory));

    public ValueTask DeleteAsync(Filename id)
    {
        ArgumentNullException.ThrowIfNull(id);

        CreateDirectories();

        File.Move(CreateFilename(id), Path.Combine(DeletedDirectory, id.ToString()));
        File.Delete(CreateFilename(id));

        return ValueTask.CompletedTask;
    }

    public async ValueTask<TEntity?> FindByIdAsync(Filename id)
    {
        ArgumentNullException.ThrowIfNull(id);

        CreateDirectories();

        var filename = CreateFilename(id);

        if (!File.Exists(filename))
        {
            return default;
        }

        var result = await File.ReadAllTextAsync(filename);
        var item = Serializer.Deserialize<TEntity?>(result);

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

    private string CreateFilename(Filename id)
    {
        return Path.Combine(BaseDirectory, id.ToString());
    }

    private void CreateDirectories()
    {
        Directory.CreateDirectory(BaseDirectory);
        Directory.CreateDirectory(DeletedDirectory);
    }
}
