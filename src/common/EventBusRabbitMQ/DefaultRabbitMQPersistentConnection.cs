using RabbitMQ.Client;
using System;

namespace ChitChatAPI.Common.EventBusRabbitMQ
{
    public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        public bool disposed;

        private readonly IConnectionFactory connectionFactory;
        private IConnection connection;
        private object sync_root = new object();

        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public bool IsConnected
        {
            get
            {
                return this.connection != null && this.connection.IsOpen && !this.disposed;
            }
        }

        public IModel CreateModel() {
            return this.connection.CreateModel();
        }

        public bool TryConnect()
        {
            lock (sync_root)
            {
                this.connection = this.connectionFactory.CreateConnection();

                return this.IsConnected;
            }
        }

        public void Dispose()
        {
            if (this.disposed) return;

            this.disposed = true;

            try
            {
                this.connection.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}