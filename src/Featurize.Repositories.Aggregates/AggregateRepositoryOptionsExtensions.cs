using Featurize.Repositories.DefaultProvider;

namespace Featurize.Repositories;

internal static class AggregateRepositoryOptionsExtensions
{

    public static void SetBaseProvider(this RepositoryOptions options, string providerName)
    {
        options["BaseProvider"] = providerName;
    }

    public static string? GetBaseProvider(this RepositoryOptions options)
    {
        if (options.TryGetValue("BaseProvider", out var value))
        {
            return value as string;
        }

        return DefaultRepositoryProvider.DefaultName;
    }
}
