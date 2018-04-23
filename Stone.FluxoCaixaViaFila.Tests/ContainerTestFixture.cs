using SimpleInjector;
using Stone.FluxoCaixaViaFila.Common;

namespace Stone.FluxoCaixaViaFila.Tests
{
    public class ContainerTestFixture
    {
        public Container Container { get; private set; }

        public ContainerTestFixture()
        {
            this.Container = new Container();
            ContainerHelper.RegisterServices(Container);
            Container.Verify();
        }
    }

}