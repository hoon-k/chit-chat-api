using RabbitMQ.Client;
using System;

namespace ChitChatAPI.Common.EventBusRabbitMQ
{
    public interface IRabbitMQPersistentConnection : IDisposable {
        IModel CreateModel();
        bool IsConnected { get; }
        bool TryConnect();
    }
}