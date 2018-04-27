
using Stone.FluxoCaixaViaFila.Domain.Assertives;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class RecebimentoSpecification : LancamentoSpecification
    {
        public RecebimentoSpecification(IFluxoCaixaRepository fluxoCaixaRepository, Lancamento lancamento) : base(fluxoCaixaRepository, lancamento)
        {
        }

        public override void Validate()
        {
            base.Validate();

            var fluxoCaixaDiario = fluxoCaixaRepository.GetPorDia(lancamento.DataLancamento);
            var totalProvisaoDiaria = fluxoCaixaDiario?.Total + lancamento.Valor + lancamento.Encargos;

            Assert.IsTrue(totalProvisaoDiaria <= LIMITE_DIARIO, $"O limite diario de {LIMITE_DIARIO:###.###,00} foi atingido, nenhum lancamento de pagamento sera aceito.");
        }
    }
}