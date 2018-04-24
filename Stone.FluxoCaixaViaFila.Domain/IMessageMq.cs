namespace Stone.FluxoCaixaViaFila.Domain
{
    public interface IMessageMq
    {
        void Put(Lancamento lancamento);
        Lancamento Get();
    }
}