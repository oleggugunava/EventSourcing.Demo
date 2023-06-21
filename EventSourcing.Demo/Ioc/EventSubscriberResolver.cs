using Trainline.EventSourcing.EventPublishing;

namespace EventSourcing.Demo.Ioc;

public class EventSubscriberResolver : IEventSubscriberResolver
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EventSubscriberResolver(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public IEnumerable<IEventSubscriber<TEvent, TAggregate>> GetSubscribers<TEvent, TAggregate>()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        return scope.ServiceProvider.GetServices<IEventSubscriber<TEvent, TAggregate>>();
    }
}
