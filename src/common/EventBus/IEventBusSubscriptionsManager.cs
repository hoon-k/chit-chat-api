using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using ChitChatAPI.Common.Event;

namespace ChitChatAPI.Common.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        // void AddSubscription(string eventName, Func<IntegrationEvent, Task> handler);
        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        // void RemoveSubscription(string eventName, Func<IntegrationEvent, Task>  handler);
        void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        // bool HasSubscriptionsForEvent(string eventName);
        IEnumerable<TH> GetHandlersForEvent<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        IEnumerable<Type> GetHandlersForEvent(string eventName);

        void Clear();

        string GetEventName<T>()
            where T : IntegrationEvent;

        bool HasSubscriptionsForEvent(string eventName);
    }
}