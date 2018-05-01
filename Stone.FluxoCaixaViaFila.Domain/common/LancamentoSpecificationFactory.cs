using System;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class LancamentoSpecificationFactory : ILancamentoSpecificationFactory
    {
        private readonly IConsolidarFluxoCaixa consolidarFluxoCaixa;

        public LancamentoSpecificationFactory(IConsolidarFluxoCaixa consolidarFluxoCaixa)
        {
            this.consolidarFluxoCaixa = consolidarFluxoCaixa;
        }

        public ILancamentoSpecification Create(Lancamento lancamento)
        {
            switch (lancamento.TipoLancamento)
            {
                case TipoLancamentoEnum.pagamento:
                    return new PagamentoSpecification(consolidarFluxoCaixa, lancamento);
                case TipoLancamentoEnum.recebimento:
                    return new RecebimentoSpecification(consolidarFluxoCaixa, lancamento);
                default:
                    break;
            }
            throw new ArgumentException("Lancamento sem tipo especificado.");
        }
    }
}