namespace Stone.FluxoCaixaViaFila.Domain
{
    public class PagamentoSpecification : LancamentoSpecification
    {
        public PagamentoSpecification(IConsolidarFluxoCaixa consolidarFluxoCaixa, Lancamento lancamento) : base(consolidarFluxoCaixa, lancamento)
        {
        }
    }
}