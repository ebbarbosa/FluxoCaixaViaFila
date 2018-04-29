using Newtonsoft.Json;
using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public abstract class MessageMq : MqBase, IPublisherMq
    {
        public abstract string QueueName { get; }

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