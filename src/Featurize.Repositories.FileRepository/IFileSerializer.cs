namespace Featurize.Repositories.FileRepository;

public interface IFileSerializer
{
    string ProviderName { get; }
    string Serialize<T>(T entity);
    T Deserialize<T>(string value);

}