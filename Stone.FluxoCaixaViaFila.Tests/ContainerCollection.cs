using Xunit;

namespace Stone.FluxoCaixaViaFila.Tests
{
    [CollectionDefinition("Container")]
    public class ContainerCollection : ICollectionFixture<ContainerTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

}