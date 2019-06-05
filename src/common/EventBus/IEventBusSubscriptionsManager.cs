using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using ChitChatAPI.Common.Event;

namespace ChitChatAPI.Common.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        void AddSubscription(string eventName, Func<IntegrationEvent, Task> handler);
        void RemoveSubscription(string eventName, Func<IntegrationEvent, Task>  handler);
        bool HasSubscriptionsForEvent(string eventName);
        List<Func<IntegrationEvent, Task>> GetHandlersForEvent(string eventName);
        void Clear();
    }
}