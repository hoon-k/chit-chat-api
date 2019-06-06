using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using ChitChatAPI.Common.Event;

namespace ChitChatAPI.Common.EventBus
{
    public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<Type>> handlers;
        private readonly List<Type> eventTypes;

        public InMemoryEventBusSubscriptionManager()
        {
            this.handlers = new Dictionary<string, List<Type>>();
            this.eventTypes = new List<Type>();
        }

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {

        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {

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