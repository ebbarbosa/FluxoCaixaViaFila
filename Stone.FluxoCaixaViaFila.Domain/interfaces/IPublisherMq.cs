namespace Stone.FluxoCaixaViaFila.Domain
{
    public interface IPublisherMq
    {
        void Put<T>(T messageDeserialized);
    }
}