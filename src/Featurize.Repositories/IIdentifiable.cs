namespace Featurize.Repositories;

/// <summary>
/// Descibes a type that is Identifiable with an Id.
/// </summary>
/// <typeparam name="TSelf">The Entity type</typeparam>
/// <typeparam name="TId"></typeparam>
public interface IIdentifiable<TSelf, TId>
    where TSelf : class
    where TId : struct
{
    /// <summary>
    /// Identifies the entity
    /// </summary>
    /// <param name="entity">The entity to get the id from</param>
    /// <returns>The Identifier of this entity.</returns>
    static abstract TId Identify(TSelf entity);
}