using Featurize.Repositories.Aggregates;

namespace Featurize.Repositories;

/// <summary>
/// Extension methods for adding Aggregates to the RepositoryProviderOptions. 
/// </summary>
public static class RepositoryProviderOptionsExtensions
{
    /// <summary>
    /// Adds a repository for an eventsourced aggregate.
    /// </summary>
    /// <typeparam name="TAggregate">The Aggregate type.</typeparam>
    /// <typeparam name="TId">The Identifier of the Aggregate.</typeparam>
    /// <param name="options">The RepositoryProviderOptions.</param>
    /// <param name="config">The Configuration to use for providing this repository</param>
    /// <returns>Returns updated Repository Provider options</returns>
    public static RepositoryProviderOptions AddAggregate<TAggregate, TId>(this RepositoryProviderOptions options, Action<RepositoryOptions>? config = null)
        where TAggregate : class, IAggregate<TAggregate, TId> 
        where TId : struct
    {
        if (!options.Providers.OfType<AggregateRepositoryProvider>().Any())
        {
            options.Providers.Add(new AggregateRepositoryProvider());
        }

        options.AddRepository<Event<TAggregate, TId>, Guid>(x => config?.Invoke(x));
        options.AddRepository<TAggregate, TId>(x =>
        {
            config?.Invoke(x);
            var provider = x.GetProviderName();
            
            x.SetBaseProvider(provider);
            
            x.Provider(AggregateRepositoryProvider.DefaultName);
        });
        
        return options;
    }
}
