﻿namespace Featurize.Repositories.Aggregates;

/// <summary>
/// Marks a class/record as an event.
/// </summary>
public interface IEvent { };

internal class Event<TAggregate, TId> 
    : IIdentifiable<Event<TAggregate, TId>, Guid>
    where TId : struct
{ 
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string AggregateName { get; set; } = typeof(TAggregate).Name;
    public TId AggregateId { get; set; }
    public int Version { get; set; }
    public IEvent? Payload { get; set; }
    public static Guid Identify(Event<TAggregate, TId> entity)
    {
        return entity.Id;
    }


    public static Event<TAggregate, TId> Create(TId aggregateId, int version, IEvent payLoad)
    {
        return new Event<TAggregate, TId>()
        {
            AggregateId = aggregateId,
            Version = version,
            Payload = payLoad,
        };
    }
}
