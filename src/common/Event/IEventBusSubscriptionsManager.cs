using System;

namespace ChitChatAPI.Common.Event
{
    public interface IEventBusSubscriptionsManager
    {
        // void AddSubscription<T, TH>()
        //    where T : IntegrationEvent
        //    where TH : IIntegrationEventHandler<T>;

        // void RemoveSubscription<T, TH>()
        //      where TH : IIntegrationEventHandler<T>
        //      where T : IntegrationEvent;
    }
}