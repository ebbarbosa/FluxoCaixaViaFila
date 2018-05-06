using System;
using Stone.FluxoCaixaViaFila.Domain.Assertives;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public abstract class LancamentoSpecification : ILancamentoSpecification
    {
        protected const decimal LIMITE_DIARIO = -20000m;
        protected IConsolidarFluxoCaixa consolidarFluxoCaixa;
        protected Lancamento lancamento;

        public LancamentoSpecification(IConsolidarFluxoCaixa consolidarFluxoCaixa, Lancamento lancamento)
        {
            this.consolidarFluxoCaixa = consolidarFluxoCaixa;
            this.lancamento = lancamento;
        }

        public virtual void Validate()
        {
            Assert.IsTrue(lancamento.DataLancamento.Date > DateTime.Today, "Lancamentos nao podem ser retroativos.");
        }
    }
}