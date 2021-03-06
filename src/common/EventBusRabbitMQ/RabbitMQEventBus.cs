using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChitChatAPI.Common.EventBus;
using ChitChatAPI.Common.Event;

namespace ChitChatAPI.Common.EventBusRabbitMQ
{
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        public const string EXCHANGE_NAME = "chichat-api";

        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly IEventBusSubscriptionsManager subManager;
        private readonly string queueName;
        private IModel consumerChannel;

        public RabbitMQEventBus(IRabbitMQPersistentConnection persistentConnection, IEventBusSubscriptionsManager subManager, string queueName)
        {
            this.persistentConnection = persistentConnection;
            this.subManager = subManager;
            this.queueName = queueName;
            this.consumerChannel = this.CreateConsumerChannel();
        }

        public void Publish(IntegrationEvent evt)
        {
            using (var channel = this.persistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: EXCHANGE_NAME,
                    type: "direct"
                );

                var message = JsonConvert.SerializeObject(evt);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                var eventName = evt.GetType().Name;
                channel.BasicPublish(
                    exchange: EXCHANGE_NAME,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                );
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            this.SubscribeInternal<T>();
            this.subManager.AddSubscription<T, TH>();
            this.StartBasicConcsume();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            this.subManager.RemoveSubscription<T, TH>();
        }

        public void Dispose()
        {
            if (this.consumerChannel != null)
            {
                this.consumerChannel.Dispose();
            }

            this.subManager.Clear();
        }

        private IModel CreateConsumerChannel() {
            if (!this.persistentConnection.IsConnected) {
                this.persistentConnection.TryConnect();
            }

            var channel = this.persistentConnection.CreateModel();

            channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME,
                type: "direct"
            );

            channel.QueueDeclare(
                queue: this.queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            return channel;
        }

        private void SubscribeInternal<T>()
            where T : IntegrationEvent
        {
            var eventName = this.subManager.GetEventName<T>();
            if (!this.subManager.HasSubscriptionsForEvent(eventName))
            {
                using (var ch = this.persistentConnection.CreateModel())
                {
                    ch.QueueBind(
                        queue: this.queueName,
                        exchange: EXCHANGE_NAME,
                        routingKey: eventName
                    );
                }
            }
        }

        private void StartBasicConcsume()
        {
            if (this.consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(this.consumerChannel);
                consumer.Received += this.ProcessEvent;
                
                this.consumerChannel.BasicConsume(
                    queue: this.queueName,
                    autoAck: false,
                    consumer: consumer
                );
            }
        }

        private async Task ProcessEvent(object sender, BasicDeliverEventArgs args)
        {
            var eventName = args.RoutingKey;
            var message = Encoding.UTF8.GetString(args.Body);

            if (this.subManager.HasSubscriptionsForEvent(eventName))
            {
                var handlers = this.subManager.GetHandlersForEvent(eventName);
                foreach (var handler in handlers) {
                    var integrationEvent = JsonConvert.DeserializeObject(message) as IntegrationEvent;
                    await ((IIntegrationEventHandler<IntegrationEvent>)handler).Handle(integrationEvent);
                }
            }
        }
    }
}