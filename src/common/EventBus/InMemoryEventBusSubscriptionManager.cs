using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using ChitChatAPI.Common.Event;

namespace ChitChatAPI.Common.EventBus
{
    public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<IIntegrationEventHandler<Type>>> handlers;

        public InMemoryEventBusSubscriptionManager()
        {
            this.handlers = new Dictionary<string, List<Func<IntegrationEvent, Task>>>();
        }

        public void AddSubscription(string eventName, Func<IntegrationEvent, Task> handler)
        {
            if (this.handlers[eventName] == null) {
                this.handlers[eventName] = new List<Func<IntegrationEvent, Task>>();
            }

            this.handlers[eventName].Add(handler);
        }

        public void RemoveSubscription(string eventName, Func<IntegrationEvent, Task> handler)
        {
            if (this.handlers[eventName] != null) {
                this.handlers[eventName].Remove(handler);
            }
        }

        public bool HasSubscriptionsForEvent(string eventName) {
            return this.handlers[eventName] != null;
        }

        public List<Func<IntegrationEvent, Task>> GetHandlersForEvent(string eventName) {
            return this.handlers[eventName];
        }

        public void Clear()
        {
            this.handlers.Clear();
        }
    }
}