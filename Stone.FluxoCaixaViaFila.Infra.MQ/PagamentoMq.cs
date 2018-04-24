namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class PagamentoMq : MessageMq
    {
        protected string _queueName = "Pagamento";

        public override string QueueName { get => _queueName; }
    }

}