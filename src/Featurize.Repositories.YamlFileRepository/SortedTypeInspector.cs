using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Featurize.Repositories.FileRepository;

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