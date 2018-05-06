namespace Stone.FluxoCaixaViaFila.Domain
{
    public class RecebimentoSpecification : LancamentoSpecification
    {
        public RecebimentoSpecification(IConsolidarFluxoCaixa consolidarFluxoCaixa, Lancamento lancamento) : base(consolidarFluxoCaixa, lancamento)
        {
        }
    }
}