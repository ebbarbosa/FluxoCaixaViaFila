using Newtonsoft.Json;
using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class RecebimentoMq : MqBase, IMessageMq
    {
        protected override string Host => "my-rabbit";

        protected override string user => "guest";

        protected override string pass => "guest";

        protected override string vhost => "/";

        protected override string exchange => "";

        protected override string routingKey => "";

        protected string _queueName = "Recebimento";

        public Lancamento Get()
        {
            var message = string.Empty;
            base.BasicGet(_queueName, msg =>
            {
                message = System.Text.Encoding.UTF8.GetString(msg.Body);
            });

            return JsonConvert.DeserializeObject<Lancamento>(message);
        }

        public void Put(Lancamento lancamento)
        {
            var message = JsonConvert.SerializeObject(lancamento);
            base.BasicPublish(_queueName, msg =>
            {
                msg = System.Text.Encoding.UTF8.GetBytes(message);
            });
        }
    }
}