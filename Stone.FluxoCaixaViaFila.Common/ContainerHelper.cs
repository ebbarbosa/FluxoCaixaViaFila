using System;
using SimpleInjector;
using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Common
{
    public class ContainerHelper
    {
        public static void RegisterServices(Container container){

            container.Register<ILancamentoFactory, LancamentoFactory>();
            container.Register<IFluxoCaixaConsumer, FluxoCaixaConsumer>();
        }
    }
}
