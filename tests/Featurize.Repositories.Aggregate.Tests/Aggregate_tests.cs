using Featurize.Repositories.Aggregates;
using FluentAssertions;

namespace Aggregate;

public class AggregateRoot_Tests
{

    public record TestEvent() : IEvent;
    public class TestAggregate : AggregateRoot<TestAggregate, Guid>
    {
        public bool ApplyCalled { get; private set; }
        public int ApplyCalledTimes { get; private set; }
        public TestAggregate(Guid id) : base(id)
        {
        }

        internal void Apply(TestEvent e)
        {
            ApplyCalled = true;
            ApplyCalledTimes++;
        }
    }

    public class Id
    {
        [Test]
        public void should_be_same_type()
        {
            var guid = Guid.NewGuid();

            var aggregate = new TestAggregate(guid);

            aggregate.Id.Should().Be(guid);
        }

    }

    public class Version
    {
        [Test]
        public void should_be_zero_on_new()
        {
            var guid = Guid.NewGuid();

            var aggregate = new TestAggregate(guid);

            aggregate.Events.Version.Should().Be(0);
        }

        [Test]
        public void should_be_same_as_latest_event()
        {
            var aggregateId = Guid.NewGuid();

            var aggregate = new TestAggregate(aggregateId);
            var events = new EventCollection<Guid>(aggregateId, new[]
            {
                new TestEvent(),
                new TestEvent(),
                new TestEvent(),
            });
            aggregate.LoadFromHistory(events);

            aggregate.Events.Version.Should().Be(3);
        }
    }

    public class ExpectedVersion
    {
        [Test]
        public void should_be_same_as_version_when_no_new_events()
        {
            var aggregateId = Guid.NewGuid();
            var events = new EventCollection<Guid>(aggregateId, new[]
            {
                new TestEvent(),
                new TestEvent(),
                new TestEvent(),
            });

            var aggregate = new TestAggregate(aggregateId);
            aggregate.LoadFromHistory(events);

            aggregate.Events.ExpectedVersion.Should().Be(aggregate.Events.Version);

        }

        [Test]
        public void should_increase_on_new_event()
        {
            var aggregateId = Guid.NewGuid();
            var events = new EventCollection<Guid>(aggregateId, new[]
            {
                new TestEvent(),
                new TestEvent(),
                new TestEvent(),
            });

            var aggregate = new TestAggregate(aggregateId);
            aggregate.LoadFromHistory(events);

            var newEvents = Random.Shared.Next(1, 9);

            for (int i = 0; i < newEvents; i++)
            {
                aggregate.RecordEvent(new TestEvent());
            }

            aggregate.Events.ExpectedVersion.Should().Be(aggregate.Events.Version + newEvents);

        }
    }

    public class LoadFromHistory
    {
        [Test]
        public void should_apply_events()
        {
            var aggregateId = Guid.NewGuid();


            var aggregate = new TestAggregate(aggregateId);
            var events = new EventCollection<Guid>(aggregateId, new[]
            {
                new TestEvent(),
                new TestEvent(),
                new TestEvent(),
            });
            aggregate.LoadFromHistory(events);

            aggregate.ApplyCalled.Should().BeTrue();
            aggregate.ApplyCalledTimes.Should().Be(events.Version);
        }
    }

    public class ApplyEvent
    {
        [Test]
        public void should_call_apply()
        {
            var aggregateId = Guid.NewGuid();
            var aggregate = new TestAggregate(aggregateId);

            aggregate.RecordEvent(new TestEvent());

            aggregate.ApplyCalled.Should().BeTrue();
            aggregate.ApplyCalledTimes.Should().Be(1);
        }
    }
}