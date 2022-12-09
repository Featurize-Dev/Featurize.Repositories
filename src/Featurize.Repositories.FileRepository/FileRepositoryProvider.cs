using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.FileRepository;

public class FileRepositoryProvider : IRepositoryProvider
{
    public FileRepositoryProvider(IFileSerializer serializer)
    {
        Serializer = serializer;
    }

    public string Name => Serializer.ProviderName;

    public bool IsConfigured { get; private set; }
    public IFileSerializer Serializer { get; }

    public void ConfigureProvider(IServiceCollection services)
    {
        services.AddSingleton<IFileSerializer>(Serializer);
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
            var serializer = sp.GetRequiredService<IFileSerializer>();
            return Activator.CreateInstance(implType, serializer, directory)!;
        };
    }

    private static string GetDirectory(RepositoryOptions options)
    {
        return options["Directory"];
    }
}
