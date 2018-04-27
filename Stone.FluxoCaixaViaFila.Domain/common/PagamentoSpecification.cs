namespace Stone.FluxoCaixaViaFila.Domain
{
    public class PagamentoSpecification : LancamentoSpecification
    {
        public PagamentoSpecification(IFluxoCaixaRepository fluxoCaixaRepository, Lancamento lancamento) : base(fluxoCaixaRepository, lancamento)
        {
        }

        public override void Validate()
        {
            base.Validate();
        }
    }
}