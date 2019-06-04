using RabbitMQ.Client;
using System;
using ChitChatAPI.Common.Event;

namespace ChitChatAPI.Common.EventBusRabbitMQ
{
    public class RabbitMQEventBus : IEventBus
    {
        public RabbitMQEventBus(IRabbitMQPersistentConnection persistentConnection, IEventBusSubscriptionsManager subManager, string queueName)
        {

        }

        public void Publish()
        {

        }

        public void Subscribe()
        {

        }

        public void Unsubscribe()
        {

        }
    }
}