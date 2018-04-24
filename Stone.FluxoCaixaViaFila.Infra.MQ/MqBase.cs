using System;
using RabbitMQ.Client;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public abstract class MqBase : IDisposable
    {
        private IConnection _Connection;
        private IModel _channel;
        private readonly object _InitializeLock = new object();

        private void Initialize()
        {
            IConnection oldConnection = _Connection;
            IModel oldChannel = _channel;
            lock (_InitializeLock)
            {
                if (_Connection == oldConnection)
                {
                    ConnectionFactory factory = new ConnectionFactory();
                    // "guest"/"guest" by default, limited to localhost connections
                    factory.UserName = user;
                    factory.Password = pass;
                    factory.VirtualHost = vhost;
                    factory.HostName = Host;

                    _Connection = factory.CreateConnection();
                }

                if (_channel == oldChannel)
                {
                    _channel = _Connection.CreateModel();
                }
            }
        }

        protected abstract string Host { get; }
        protected abstract string user { get; }
        protected abstract string pass { get; }
        protected abstract string vhost { get; }
        protected abstract string exchange { get; }
        protected abstract string routingKey { get; }


        protected void ExecuteWithRetryOnConnectionError(string queueName, string exchange, string routingKey, Action<IModel> action)
        {
            if (_Connection == null || _channel == null)
            {
                Initialize();
            }

            Action execute = () =>
            {
                _channel.QueueBind(queueName, exchange, routingKey);
                action(_channel);
            };
            try
            {
                execute();
            }
            catch (RabbitMQ.Client.Exceptions.ConnectFailureException)
            {
                Initialize();
                execute();
            }
            catch
            {
                throw;
            }
            finally
            {
                _channel?.Close();
            }
        }


        protected void BasicGet(string queueName, Action<BasicGetResult> readMessage)
        {
            Action<IModel> get = c => {
                bool noAck = false;
                BasicGetResult result = c.BasicGet(queueName, noAck);
                if (result == null)
                {
                    // No message available at this time.
                }
                else
                {
                    IBasicProperties props = result.BasicProperties;
                    byte[] body = result.Body;

                    readMessage(result);

                    c.BasicAck(result.DeliveryTag, false);
                }
            };

            ExecuteWithRetryOnConnectionError(queueName, exchange, routingKey, get);
        }

        protected void BasicPublish(string queueName, Action<byte[]> populateMessage)
        {
            Action<IModel> put = c =>
            {
                byte[] messageBodyBytes = { };
                populateMessage(messageBodyBytes);
                c.BasicPublish(exchange, routingKey, null, messageBodyBytes);
            };

            ExecuteWithRetryOnConnectionError(queueName, exchange, routingKey, put);
        }

        public void Close()
        {
            _channel?.Close();
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}