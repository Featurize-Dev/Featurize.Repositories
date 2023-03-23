using Featurize.Repositories.DefaultProvider;

namespace Featurize.Repositories.Aggregates;
internal static class AggregateRepositoryOptionsExtensions
{
    public static void SetBaseProvider(this RepositoryOptions options, string providerName)
    {
        options["BaseProvider"] = providerName;
    }

    public static string GetBaseProvider(this RepositoryOptions options)
    {
        if (options.ContainsKey("BaseProvider"))
        {
            return options["BaseProvider"];
        }

        return DefaultRepositoryProvider.DefaultName;
    }
}
