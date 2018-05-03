using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public abstract class MqBase : IDisposable
    {
        private IModel channel;

        protected void BasicConsumer(string queueName, Action<string> readMessage)
        {
            Register();

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
            channel.BasicConsume(queueName,
                                 false,
                                 consumer);

        }

        protected void BasicPublish(string queueName, byte[] message)
        {
            Register();

            channel.QueueDeclare(queue: queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: properties,
                                 body: message);
        }

        public void Register()
        {
            channel = RabbitMqConnectionHelper.GetModel();
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        public void Dispose()
        {
            channel?.Close();
        }
    }
}