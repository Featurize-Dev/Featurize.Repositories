namespace Featurize.Repositories.EntityFramework;
public static class EntityFrameworkRepositoryOptionsExtensions
{



    public static RepositoryProviderOptions AddEntityFramework(this RepositoryProviderOptions o, Action<EntityFrameworkRepositoryProviderOptions> options)
    {
        var opt = new EntityFrameworkRepositoryProviderOptions();
        options.Invoke(opt);
        o.Providers.Add(new EntityFrameworkRepositoryProvider(opt));
        return o;
    }
      

    public static RepositoryOptions UseContext<TDbContext>(this RepositoryOptions options)
    {
        options[nameof(UseContext)] = typeof(TDbContext);
        return options;
    }
}
