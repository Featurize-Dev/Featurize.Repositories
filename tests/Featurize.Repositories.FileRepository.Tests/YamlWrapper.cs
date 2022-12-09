using Featurize.Repositories.FileRepository;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Featurize.Repositories.FileRepository.Tests;
public class YamlWrapper : IFileSerializer
{
    public static YamlWrapper Create() => new YamlWrapper();

    private ISerializer _serializer;
    private IDeserializer _deserializer;

    public string ProviderName => "Yaml";

    public YamlWrapper()
    {
        _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeInspector(x => new SortedTypeInspector(x))
            .ConfigureDefaultValuesHandling(
                DefaultValuesHandling.OmitNull |
                DefaultValuesHandling.OmitDefaults |
                DefaultValuesHandling.OmitEmptyCollections)
            .Build();

        _deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .WithTypeInspector(x => new SortedTypeInspector(x))
           .Build();
    }

    public T Deserialize<T>(string value)
    {
        return _deserializer.Deserialize<T>(value);
    }

    public string Serialize<T>(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        return _serializer.Serialize(entity);
    }
}

public sealed class SortedTypeInspector : TypeInspectorSkeleton
{
    private readonly ITypeInspector innerTypeDescriptor;

    public SortedTypeInspector(ITypeInspector innerTypeDescriptor)
    {
        this.innerTypeDescriptor = innerTypeDescriptor;
    }

    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
    {
        return innerTypeDescriptor
            .GetProperties(type, container)
            .OrderBy(p => p.Name);
    }

}
