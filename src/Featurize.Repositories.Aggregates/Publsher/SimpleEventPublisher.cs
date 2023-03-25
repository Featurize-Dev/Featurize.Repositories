using Microsoft.Extensions.DependencyInjection;

namespace Featurize.Repositories.Aggregates.Publsher;

public interface IEventPublisher
{
    Task Publish(IEvent e);
}

public interface IEventHandler<TEvent>
    where TEvent : IEvent
{
    ValueTask Handle(TEvent e);
}

public class SimpleEventPublisher : IEventPublisher
{
    private readonly IServiceProvider _provider;

    public SimpleEventPublisher(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task Publish(IEvent e)
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(e.GetType());
        var methodInfo = handlerType.GetMethod("Handle")!;
        var handlers = _provider.GetServices(handlerType);

        await Parallel.ForEachAsync(handlers, async (handler, ct) =>
        {
            ValueTask result = (ValueTask)methodInfo?.Invoke(handler, new[] { e })!;
            await result;
        });
    }
}
