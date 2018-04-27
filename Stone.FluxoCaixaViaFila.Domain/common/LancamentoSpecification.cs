using System;
using Stone.FluxoCaixaViaFila.Domain.Assertives;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public abstract class LancamentoSpecification : ILancamentoSpecification
    {
        protected const decimal LIMITE_DIARIO = 20000m;
        protected IFluxoCaixaRepository fluxoCaixaRepository;
        protected Lancamento lancamento;

        public LancamentoSpecification(IFluxoCaixaRepository fluxoCaixaRepository, Lancamento lancamento)
        {
            this.fluxoCaixaRepository = fluxoCaixaRepository;
            this.lancamento = lancamento;
        }

        public virtual void Validate()
        {
            Assert.IsTrue(lancamento.DataLancamento > DateTime.Today, "Lancamentos nao podem ser retroativos.");
        }
    }
}