using Featurize.Repositories.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories.Aggregate.Tests;
public record ChangeNameEvent(string Name) : IEvent;
public class TestAggregate : AggregateRoot<TestAggregate, Guid>,
    IAggregate<TestAggregate, Guid>
{
    private TestAggregate(Guid id) : base(id) { }

    public string Name { get; private set; } = string.Empty;

    public static TestAggregate Create(Guid id)
    {
        return new TestAggregate(id);
    }

    public void ChangeName(string name)
    {
        ApplyEvent(new ChangeNameEvent(name));
    }

    private void Apply(ChangeNameEvent e)
    {
        Name = e.Name;
    }

    public static Guid Identify(TestAggregate entity)
    {
        return entity.Id;
    }
}
