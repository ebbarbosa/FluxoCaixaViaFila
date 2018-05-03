using Newtonsoft.Json;
using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public abstract class MessageMq : MqBase, IPublisherMq
    {
        public abstract string QueueName { get; }

        public void Put<T>(T messageDeserialized)
        {
            var message = JsonConvert.SerializeObject(messageDeserialized);
            var bytes = System.Text.Encoding.UTF8.GetBytes(message);
            base.BasicPublish(QueueName, bytes);
        }
    }
}