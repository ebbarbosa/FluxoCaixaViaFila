using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public abstract class MqBase : IDisposable
    {
        private IConnection connection;
        private IModel channel;
        private readonly object _InitializeLock = new object();

        protected void BasicConsume(string queueName, Action<string> readMessage)
        {
            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                readMessage(message);

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

        }

        protected void BasicPublish(string queueName, Action<byte[]> populateMessage)
        {
            channel.QueueDeclare(queue: queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);


            byte[] messageBodyBytes = { };
            populateMessage(messageBodyBytes);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: properties,
                                 body: messageBodyBytes);
        }

        public void Register()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();



            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        public void Deregister()
        {
            channel?.Close();
            connection?.Close();
        }

        public void Dispose()
        {
            Deregister();
        }
    }
}