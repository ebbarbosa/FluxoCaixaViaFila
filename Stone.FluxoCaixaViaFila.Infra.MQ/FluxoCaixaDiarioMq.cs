using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class FluxoCaixaDiarioMq : MessageMq, IFluxoCaixaDiarioMq
    {
        protected string _queueName = "FluxoCaixa";

        public override string QueueName { get => _queueName; }
    }
}