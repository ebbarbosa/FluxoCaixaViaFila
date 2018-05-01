namespace Stone.FluxoCaixaViaFila.Domain
{
    public interface IPublisherMq
    {
        void Put(object messageDeserialized);
    }
}