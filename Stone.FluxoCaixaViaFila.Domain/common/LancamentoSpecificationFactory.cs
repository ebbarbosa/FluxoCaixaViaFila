using System;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class LancamentoSpecificationFactory : ILancamentoSpecificationFactory
    {
        private readonly IFluxoCaixaRepository fluxoCaixaRepository;

        public LancamentoSpecificationFactory(IFluxoCaixaRepository fluxoCaixaRepository)
        {
            this.fluxoCaixaRepository = fluxoCaixaRepository;
        }

        public ILancamentoSpecification Create(Lancamento lancamento)
        {
            switch (lancamento.TipoLancamento)
            {
                case TipoLancamentoEnum.pagamento:
                    return new PagamentoSpecification(fluxoCaixaRepository, lancamento);
                case TipoLancamentoEnum.recebimento:
                    return new RecebimentoSpecification(fluxoCaixaRepository, lancamento);
                default:
                    break;
            }
            throw new ArgumentException("Lancamento sem tipo especificado.");
        }
    }
}