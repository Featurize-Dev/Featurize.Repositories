namespace Featurize.Repositories.Aggregates;

public interface IEvent { };
public class Event<TId> 
    : IIdentifiable<Event<TId>, Guid>
    where TId : struct
{ 
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string AggregateId { get; set; }
    public int Version { get; set; }
    public IEvent Payload { get; set; }
    public static Guid Identify(Event<TId> entity)
    {
        return entity.Id;
    }
}
