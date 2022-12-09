using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.FileRepository;

public class FileRepositoryProvider : IRepositoryProvider
{
    public FileRepositoryProvider()
    {
    }

    public string Name => "Yaml";

    public bool IsConfigured { get; private set; }

    public void ConfigureProvider(IServiceCollection services)
    {
        //services.AddScoped(x => new SerializerBuilder()
        //    .WithNamingConvention(CamelCaseNamingConvention.Instance)
        //    .WithTypeInspector(x => new SortedTypeInspector(x))
        //    .ConfigureDefaultValuesHandling(
        //        DefaultValuesHandling.OmitNull |
        //        DefaultValuesHandling.OmitDefaults |
        //        DefaultValuesHandling.OmitEmptyCollections)
        //    .Build());

        //services.AddScoped(x => new DeserializerBuilder()
        //       .WithNamingConvention(CamelCaseNamingConvention.Instance)
        //       .WithTypeInspector(x => new SortedTypeInspector(x))
        //       .Build());

        IsConfigured = true;
    }

    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var directory = GetDirectory(info.Options);
        var serviceType = typeof(IRepository<,>).MakeGenericType(info.EntityType, typeof(Filename));
        var yamlServiceType = typeof(IFileRepository<>).MakeGenericType(info.EntityType);
        var implType = typeof(FileRepository<>).MakeGenericType(info.EntityType);

        services.AddScoped(serviceType, CreateRepository(directory, implType));
        services.AddScoped(yamlServiceType, CreateRepository(directory, implType));
    }

    private static Func<IServiceProvider, object> CreateRepository(string directory, Type implType)
    {
        return sp =>
        {
            var serializer = sp.GetRequiredService<ISerializer>();
            var deserialilzer = sp.GetRequiredService<IDeserializer>();
            return Activator.CreateInstance(implType, serializer, deserialilzer, directory)!;
        };
    }

    private static string GetDirectory(RepositoryOptions options)
    {
        return options["Directory"];
    }
}
