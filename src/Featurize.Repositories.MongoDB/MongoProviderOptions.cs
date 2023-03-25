using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories.MongoDB;

/// <summary>
/// Mongo provider options
/// </summary>
public class MongoProviderOptions
{
    /// <summary>
    /// Name of the provider
    /// </summary>
    public string Name { get; set; } = MongoRepositoryProvider.DefaultName;
    /// <summary>
    /// The Connection String of this provider
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Allowed types by the default ObjectSerializer.
    /// </summary>
    public Func<Type, bool> AllowedTypes { get; set; } = ObjectSerializer.DefaultAllowedTypes;

    /// <summary>
    /// List of Serializer Providers for your custom types.
    /// </summary>
    public List<IBsonSerializationProvider> SerializationProviders { get; set; } = new();

    /// <summary>
    /// Types to use for LookupClassMap.
    /// </summary>
    public IEnumerable<Type> ClassMaps { get; set; } = Array.Empty<Type>();

}
