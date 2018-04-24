using Newtonsoft.Json;
using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public abstract class MessageMq : MqBase, IMessageMq
    {
        protected override string Host => "my-rabbit";

        protected override string user => "guest";

        protected override string pass => "guest";

        protected override string vhost => "/";

        protected override string exchange => "";

        protected override string routingKey => "";

        public abstract string QueueName { get; }

        public Lancamento Get()
        {
            var message = string.Empty;
            base.BasicGet(QueueName, msg =>
            {
                message = System.Text.Encoding.UTF8.GetString(msg.Body);
            });

            return JsonConvert.DeserializeObject<Lancamento>(message);
        }

        public void Put(Lancamento lancamento)
        {
            var message = JsonConvert.SerializeObject(lancamento);
            base.BasicPublish(QueueName, msg =>
            {
                msg = System.Text.Encoding.UTF8.GetBytes(message);
            });
        }
    }

}