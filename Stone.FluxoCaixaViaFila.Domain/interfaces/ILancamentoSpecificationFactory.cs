namespace Stone.FluxoCaixaViaFila.Domain
{
    public interface ILancamentoSpecificationFactory
    {
        ILancamentoSpecification Create(Lancamento lancamento);
    }
}