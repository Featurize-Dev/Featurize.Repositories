using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.FileRepository;

/// <summary>
/// A File Repository Provider.
/// </summary>
/// <seealso cref="Featurize.Repositories.IRepositoryProvider" />
public class FileRepositoryProvider : IRepositoryProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileRepositoryProvider"/> class.
    /// </summary>
    /// <param name="serializer">The serializer.</param>
    /// <param name="providerName">Name of the provider.</param>
    public FileRepositoryProvider(IFileSerializer serializer, string providerName)
    {
        Serializer = serializer;
        Name = providerName;
    }

    /// <summary>
    /// The name of the provider.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Is the provider configured.
    /// </summary>
    public bool IsConfigured { get; private set; }
    /// <summary>
    /// Gets the serializer.
    /// </summary>
    /// <value>
    /// The serializer.
    /// </value>
    public IFileSerializer Serializer { get; }

    /// <summary>
    /// Configures services required for this provider.
    /// </summary>
    /// <param name="services">The service collection <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
    public void ConfigureProvider(IServiceCollection services)
    {
        services.AddSingleton<IFileSerializer>(Serializer);
        IsConfigured = true;
    }

    /// <summary>
    /// Configures the services for the repository.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="info"></param>
    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var directory = GetDirectory(info.Options);
        var repsitoryType = typeof(IRepository<,>).MakeGenericType(info.EntityType, typeof(Filename));
        var serviceType = typeof(IEntityRepository<,>).MakeGenericType(info.EntityType, typeof(Filename));
        var yamlServiceType = typeof(IFileRepository<>).MakeGenericType(info.EntityType);
        var implType = typeof(FileRepository<>).MakeGenericType(info.EntityType);

        services.AddScoped(repsitoryType, CreateRepository(directory, implType));
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
        if(options.TryGetValue("Directory", out object? value))
        {
            return value as string ?? string.Empty;
        }

        return string.Empty;
    }
}
