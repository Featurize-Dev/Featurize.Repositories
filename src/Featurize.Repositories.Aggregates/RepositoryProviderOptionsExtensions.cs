using Featurize.Repositories.Aggregates;

namespace Featurize.Repositories;
public static class RepositoryProviderOptionsExtensions
{
    public static RepositoryProviderOptions AddAggregate<TAggregate, TId>(this RepositoryProviderOptions options, Action<RepositoryOptions>? config = null)
        where TAggregate : class, IAggregate<TAggregate, TId> 
        where TId : struct
    {
        if(!options.Providers.OfType<AggregateRepositoryProvider>().Any())
        {
            options.Providers.Add(new AggregateRepositoryProvider());
        }

        options.AddRepository<Event<TAggregate, TId>, Guid>(x => config?.Invoke(x));
        options.AddRepository<TAggregate, TId>(x =>
        {
            var provider = x.GetProviderName();
            x.SetBaseProvider(provider);
            x.Provider(AggregateRepositoryProvider.DefaultName);
        });
        
        return options;
    }
}
