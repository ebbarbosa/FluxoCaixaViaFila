using System;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{

    public class ConsumerRecebimentosService : ConsumerLancamentoService
    {
        public ConsumerRecebimentosService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override string QueueName => "Recebimento";
    }
}