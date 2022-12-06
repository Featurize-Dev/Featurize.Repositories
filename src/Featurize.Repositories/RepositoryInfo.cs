namespace Featurize.Repositories;

/// <summary>
/// Type that hold the repository data.
/// </summary>
/// <param name="EntityType">The type of the entity</param>
/// <param name="IdType">The type of the Id of the entity</param>
/// <param name="Options">The options for the Repository.</param>
public record RepositoryInfo(Type EntityType, Type IdType, RepositoryOptions Options);