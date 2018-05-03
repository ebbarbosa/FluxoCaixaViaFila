using RabbitMQ.Client;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class RabbitMqConnectionHelper
    {
        private static IConnection _Connection;
        private static readonly object _InitializeLock = new object();

        private static void Initialize()
        {
            IConnection oldConnection = _Connection;
            lock (_InitializeLock)
            {
                if (_Connection == oldConnection)
                {
                    // "guest"/"guest" by default, limited to localhost connections
                    var factory = new ConnectionFactory()
                    {
                        HostName = "192.168.99.100",
                        Port = 32777,
                        UserName = "guest",
                        Password = "guest",
                        RequestedHeartbeat = 60,
                        Ssl =
                        {
                            ServerName = "192.168.99.100",
                            Enabled = false
                        }
                    };
                    _Connection = factory.CreateConnection();
                }
            }
        }

        public static IModel GetModel()
        {
            Initialize();
            return _Connection.CreateModel();
        }

        public static void Dispose()
        {
            _Connection?.Close();
        }
    }
}