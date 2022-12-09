namespace Featurize.Repositories.FileRepository;

/// <summary>
/// A Repository for serializing files in a directory
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <seealso cref="Featurize.Repositories.FileRepository.IFileRepository&lt;TEntity&gt;" />
public class FileRepository<TEntity> : IFileRepository<TEntity>
    where TEntity : class, IIdentifiable<TEntity, Filename>, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileRepository{TEntity}"/> class.
    /// </summary>
    /// <param name="serializer">The serializer.</param>
    /// <param name="path">The path.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public FileRepository(IFileSerializer serializer, string path)
    {
        ArgumentNullException.ThrowIfNull(serializer);
        ArgumentNullException.ThrowIfNull(path);

        Serializer = serializer;
        BaseDirectory = Path.Combine(path, typeof(TEntity).Name.ToLower());
        DeletedDirectory = Path.Combine(BaseDirectory, "deleted");

        CreateDirectories();
    }

    /// <summary>
    /// Gets the serializer.
    /// </summary>
    /// <value>
    /// The serializer.
    /// </value>
    public IFileSerializer Serializer { get; }
    /// <summary>
    /// Gets the base directory.
    /// </summary>
    /// <value>
    /// The base directory.
    /// </value>
    public string BaseDirectory { get; }
    /// <summary>
    /// Gets the deleted directory.
    /// </summary>
    /// <value>
    /// The deleted directory.
    /// </value>
    public string DeletedDirectory { get; }

    /// <summary>
    /// Gets the query.
    /// </summary>
    /// <value>
    /// The query.
    /// </value>
    public IQuery<TEntity> Query => new DirectoryQueryable<TEntity>(Serializer, Directory.EnumerateFiles(BaseDirectory));

    /// <summary>
    /// Removes a entity from the underlying storage.
    /// </summary>
    /// <param name="id">Id of the entity to remove</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public ValueTask DeleteAsync(Filename id)
    {
        ArgumentNullException.ThrowIfNull(id);

        File.Move(CreateFilename(id), Path.Combine(DeletedDirectory, id.ToString()));
        File.Delete(CreateFilename(id));

        return ValueTask.CompletedTask;
    }
    /// <summary>
    /// Finds the by identifier asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public async ValueTask<TEntity?> FindByIdAsync(Filename id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var filename = CreateFilename(id);

        if (!File.Exists(filename))
        {
            return default;
        }

        var result = await File.ReadAllTextAsync(filename);
        var item = Serializer.Deserialize<TEntity?>(result);

        return item;
    }

    /// <summary>
    /// Saves the entity in the underlying storage.
    /// </summary>
    /// <param name="entity">The entity to save.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public async ValueTask SaveAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

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
