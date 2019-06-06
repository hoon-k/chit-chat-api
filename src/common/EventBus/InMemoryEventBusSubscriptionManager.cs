using System;
using System.Collections.Generic;
using System.Linq;
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
            var eventName = this.GetEventName<T>();
            
            if (!this.HasSubscriptionsForEvent(eventName))
            {
                this.handlers[eventName] = new List<Type>();
            }

            this.handlers[eventName].Add(typeof(TH));
        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = this.GetEventName<T>();

            if (!this.HasSubscriptionsForEvent(eventName))
            {
                return;
            }

            var handler = this.handlers[eventName].SingleOrDefault((hdlrType) => hdlrType == typeof(TH));
            if (handler != null) {
                this.handlers[eventName].Remove(handler);
            }
        }

        public bool HasSubscriptionsForEvent(string eventName) {
            return this.handlers[eventName] != null;
        }

        public IEnumerable<Type> GetHandlersForEvent(string eventName)
        {
            return this.handlers[eventName];
        }

        public IEnumerable<TH> GetHandlersForEvent<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = this.GetEventName<T>();
            return this.handlers[eventName] as List<TH>;
        }

        public void Clear()
        {
            this.handlers.Clear();
        }

        public string GetEventName<T>()
            where T : IntegrationEvent
        {
            return typeof(T).Name;
        }
    }
}