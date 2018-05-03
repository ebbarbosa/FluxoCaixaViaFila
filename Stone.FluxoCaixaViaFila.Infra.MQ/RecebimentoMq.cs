namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class RecebimentoMq : MessageMq
    {
        protected string _queueName = "Recebimento";

        public override string QueueName { get => _queueName; }
	}

}