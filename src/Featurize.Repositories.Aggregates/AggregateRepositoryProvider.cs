using Featurize.Repositories.Aggregates.Publsher;
using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.Aggregates;
internal class AggregateRepositoryProvider : IRepositoryProvider
{
    public static string DefaultName => nameof(AggregateRepositoryProvider);

    public string Name => DefaultName;

    public bool IsConfigured { get; }

    public void ConfigureProvider(IServiceCollection services)
    {
        services.AddScoped<IEventPublisher, SimpleEventPublisher>();
    }

    public void ConfigureRepository(IServiceCollection services, RepositoryInfo info)
    {
        var projectorType = info.Options.GetProjectorType();

        if(projectorType != null)
        {
            services.AddScoped(projectorType);
            RegisterEventHandlers(services, projectorType);
        }

        var implType = typeof(AggregateRepository<,>).MakeGenericType(info.EntityType, info.IdType);
        var serviceType = typeof(IRepository<,>).MakeGenericType(info.EntityType, info.IdType);

        services.AddScoped(serviceType, implType);
    }

    private static void RegisterEventHandlers(IServiceCollection services, Type projectorType)
    {
        var interfaces = projectorType.GetInterfaces()
            .Where(x=>x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEventHandler<>));

        foreach (var i in interfaces)
        {
            services.AddScoped(i, projectorType);
        }
    }
}
