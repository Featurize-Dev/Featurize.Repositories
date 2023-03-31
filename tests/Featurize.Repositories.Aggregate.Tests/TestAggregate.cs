using Featurize.Repositories.Aggregates;

namespace Featurize.Repositories.Aggregate.Tests;
public record ChangeNameEvent(string Name) : IEvent;
public class TestAggregate : AggregateRoot<TestAggregate, Guid>,
    IAggregate<TestAggregate, Guid>
{
    private TestAggregate(Guid id) : base(id) { }

    public string Name { get; private set; } = string.Empty;

    public static TestAggregate Create(Guid id)
    {
        var aggregate = new TestAggregate(id);
        aggregate.RecordEvent(new ChangeNameEvent("Default Value"));
        return aggregate;
    }

    public void ChangeName(string name)
    {
        RecordEvent(new ChangeNameEvent(name));
    }

    internal void Apply(ChangeNameEvent e)
    {
        Name = e.Name;
    }

    public static Guid Identify(TestAggregate entity)
    {
        return entity.Id;
    }
}
