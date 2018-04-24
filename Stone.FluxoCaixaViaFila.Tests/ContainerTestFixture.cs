using SimpleInjector;
using SimpleInjector.Lifestyles;
using Stone.FluxoCaixaViaFila.Common;
using Stone.FluxoCaixaViaFila.Domain;
using Stone.FluxoCaixaViaFila.Infra.MQ;

namespace Stone.FluxoCaixaViaFila.Tests
{
    public class ContainerTestFixture
    {
        public Container Container { get; private set; }

        public ContainerTestFixture()
        {
            this.Container = new Container();
            ContainerHelper.RegisterServices(Container, c => {
                c.Register<ILancamentoMqFactory, LancamentoMqFactory>(Lifestyle.Singleton);
            });
            Container.Verify();
        }
    }

}