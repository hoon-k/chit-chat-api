using System;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using ChitChatAPI.Common.Event;

namespace ChitChatAPI.Common.EventBus
{
    public interface IEventBus
    {
        void Publish(string eventName, IntegrationEvent evt);
        void Subscribe(string eventName, Func<IntegrationEvent, Task> handler);
        void Unsubscribe(string eventName, Func<IntegrationEvent, Task> handler);
    }
}