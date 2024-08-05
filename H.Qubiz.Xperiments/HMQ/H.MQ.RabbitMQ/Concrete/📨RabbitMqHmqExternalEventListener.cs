using H.MQ.Abstractions;
using H.Necessaire;
using H.MQ.Core;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using H.Necessaire.Serialization;

namespace H.MQ.RabbitMQ.Concrete
{
    [ID("RabbitMQ")]
    [Alias("rabbit-mq", "rabbit")]
    internal class RabbitMqHmqExternalEventListener : ImAnHmqExternalEventListener, ImADependency, IDisposable
    {
        string routingKey = "hmq";
        string exchange = "hmq";
        ConnectionFactory rabbitMqConnectionFactory;
        IConnection rabbitMqConenction;
        IModel rabbitMqChannel;
        EventingBasicConsumer eventConsumer;
        ImAnHmqEventRiser internalEventRiser;
        ImALogger logger;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            ConfigNode config
                = dependencyProvider
                .GetRuntimeConfig()
                ?.Get("HMQ")
                ?.Get("RabbitMQ")
                ;

            rabbitMqConnectionFactory = new ConnectionFactory
            {
                HostName = config?.Get("HostName")?.ToString(),
                VirtualHost = config?.Get("VirtualHost")?.ToString(),
                UserName = config?.Get("UserName")?.ToString(),
                Password = config?.Get("Password")?.ToString(),
            };

            internalEventRiser = dependencyProvider.Build<ImAnHmqEventRiser>("internal");
            logger = dependencyProvider.GetLogger<RabbitMqHmqExternalEventListener>();
        }

        public Task<OperationResult> Start()
        {
            rabbitMqConenction = rabbitMqConnectionFactory.CreateConnection();
            rabbitMqChannel = rabbitMqConenction.CreateModel();

            rabbitMqChannel.ExchangeDeclare(exchange, ExchangeType.Direct);
            QueueDeclareOk queue = rabbitMqChannel.QueueDeclare();

            rabbitMqChannel
                .QueueBind(
                    queue: queue.QueueName,
                    exchange: exchange,
                    routingKey: routingKey
                );

            eventConsumer = new EventingBasicConsumer(rabbitMqChannel);

            eventConsumer.Received += EventConsumer_Received;

            rabbitMqChannel
                .BasicConsume(
                    queue: queue.QueueName,
                    autoAck: true,
                    consumer: eventConsumer
                );

            return OperationResult.Win().AsTask();
        }

        public Task<OperationResult> Stop()
        {
            eventConsumer.Received -= EventConsumer_Received;
            rabbitMqChannel.Dispose();
            rabbitMqConenction.Dispose();
            return OperationResult.Win().AsTask();
        }

        public void Dispose()
        {
            new Action(() =>
            {
                Stop().ConfigureAwait(false).GetAwaiter().GetResult();

            }).TryOrFailWithGrace();
        }


        private async void EventConsumer_Received(object sender, BasicDeliverEventArgs args)
        {
            byte[] body = args.Body.ToArray();
            string hmqEventAsJsonString = Encoding.UTF8.GetString(body);
            HmqEvent hmqEvent = hmqEventAsJsonString.TryJsonToObject<HmqEvent>().ThrowOnFailOrReturn().ToWellTypedEventDataFromJson();
            await internalEventRiser.Raise(hmqEvent);
        }
    }
}
