using System;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class ConsumerPagamentosService : ConsumerLancamentoService
    {
        public ConsumerPagamentosService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override string QueueName => "Pagamento";
    }
}