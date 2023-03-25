using Featurize.Repositories.DefaultProvider;

namespace Featurize.Repositories;

public static class RepositoryOptionsExtensions
{
    public static void Projector<T>(this RepositoryOptions o) => o.ProjectorType(typeof(T));

    public static void ProjectorType(this RepositoryOptions options, Type type)
    {
        options.Add("ProjectorType", type.AssemblyQualifiedName!);
    }
}

internal static class AggregateRepositoryOptionsExtensions
{

    public static Type? GetProjectorType(this RepositoryOptions options)
    {
        if (options.TryGetValue("ProjectorType", out var value))
            return Type.GetType(value);

        return null;
    }

    public static void SetBaseProvider(this RepositoryOptions options, string providerName)
    {
        options["BaseProvider"] = providerName;
    }

    public static string GetBaseProvider(this RepositoryOptions options)
    {
        if (options.TryGetValue("BaseProvider", out var value))
        {
            return value;
        }

        return DefaultRepositoryProvider.DefaultName;
    }
}
