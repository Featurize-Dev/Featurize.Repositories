using Featurize.Repositories.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Featurize.Repositories.Aggregates.Publsher;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Featurize.Repositories.Aggregate.Tests;
internal class EventPublisher
{
    public static int Counter;
    public static int Cnt => Interlocked.Increment(ref Counter);
    public static void WriteTrace(string from) => Trace.WriteLine($"{Cnt}:{from}:{Thread.CurrentThread.Name ?? "ThreadPool"}");


    [Test]
    public void should_publish_events()
    {
        var services = new ServiceCollection();
        var handler1 = new Handler1();
        var handler2 = new Handler2();

        services.AddSingleton<IEventHandler<TestEvent>>(handler1);
        services.AddSingleton<IEventHandler<TestEvent>>(handler2);

        var provider = services.BuildServiceProvider();
        
        var publisher = new SimpleEventPublisher(provider);

        var e = new TestEvent();

        publisher.Publish(e);

    }
    
}

public record TestEvent() : IEvent;
public class Handler1 : IEventHandler<TestEvent>
{
    public bool Called = false;
    public ValueTask Handle(TestEvent e)
    {
        EventPublisher.WriteTrace("Handler 1");
        Called = true;
        return ValueTask.CompletedTask;
    }
}

public class Handler2 : IEventHandler<TestEvent>
{
    public bool Called = false;
    public ValueTask Handle(TestEvent e)
    {
        EventPublisher.WriteTrace("Handler 2");
        Called = true;
        return ValueTask.CompletedTask;
    }
}


