using System;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using ChitChatAPI.Common.Event;

namespace ChitChatAPI.Common.EventBus
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent evt);
        // void Subscribe(string eventName, IIntegrationEventHandler<IntegrationEvent> handler);
        // void Unsubscribe(string eventName, IIntegrationEventHandler<IntegrationEvent> handler);

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
            
    }
}