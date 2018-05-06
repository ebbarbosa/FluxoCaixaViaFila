
using System.Linq;
using Stone.FluxoCaixaViaFila.Domain.Assertives;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class PagamentoSpecification : LancamentoSpecification
    {
        public PagamentoSpecification(IConsolidarFluxoCaixa consolidarFluxoCaixa, Lancamento lancamento) : base(consolidarFluxoCaixa, lancamento)
        {
        }

        public override void Validate()
        {
            base.Validate();

            var fluxoCaixa = consolidarFluxoCaixa.ConsolidarMes();
            var fluxoCaixaDiario = fluxoCaixa.FirstOrDefault(f => f.Data.Date.Equals(lancamento.DataLancamento.Date));
            var totalProvisaoDiaria = (lancamento.Valor + lancamento.Encargos) * -1 + fluxoCaixaDiario?.Total;

            Assert.IsTrue(totalProvisaoDiaria > LIMITE_DIARIO, $"O limite diario de {LIMITE_DIARIO:###.###,00} foi atingido, nenhum lancamento de pagamento sera aceito.");
        }
    }
}