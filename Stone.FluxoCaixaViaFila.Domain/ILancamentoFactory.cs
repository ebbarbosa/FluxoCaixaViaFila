namespace Stone.FluxoCaixaViaFila.Domain
{
    public interface ILancamentoFactory
    {
        Lancamento Create(string jsonLancamento);
    }
}