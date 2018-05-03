using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using SimpleInjector;
using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Common
{
    public class ContainerHelper
    {
        public static void RegisterServices(Container container, Action<Container> setInfra) {

            container.Register<ILancamentoFactory, LancamentoFactory>();
            container.Register<IConsolidarFluxoCaixa, ConsolidarFluxoCaixa>(Lifestyle.Singleton);
            container.Register<ILancamentoRouter, LancamentoRouter>();
            container.Register<ILancamentoSpecificationFactory, LancamentoSpecificationFactory>();

            setInfra(container);
        }
    }
}
