using System;
using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class LancamentoMqFactory : ILancamentoMqFactory
    {
        public IMessageMq Create(TipoLancamentoEnum tipoLancamento)
        {
            switch (tipoLancamento)
            {
                case TipoLancamentoEnum.recebimento:
                    return new RecebimentoMq();
                case TipoLancamentoEnum.pagamento:
                default:
                    break;
            }
            throw new ArgumentException($"Nao existe fila para o tipo de lancamento {tipoLancamento}.");
        }
    }
}