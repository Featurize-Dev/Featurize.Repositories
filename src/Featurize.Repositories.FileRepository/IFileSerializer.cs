namespace Featurize.Repositories.FileRepository;

/// <summary>
/// Defines a FileSerializer for serializing Entities to file.
/// </summary>
public interface IFileSerializer
{
    /// <summary>
    /// Serializes the specified entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    string Serialize<T>(T entity);
    /// <summary>
    /// Deserializes the specified value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    T Deserialize<T>(string value);

}