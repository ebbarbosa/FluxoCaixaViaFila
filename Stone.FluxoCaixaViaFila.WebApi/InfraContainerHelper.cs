﻿using System;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using Stone.FluxoCaixaViaFila.Domain;
using Stone.FluxoCaixaViaFila.Infra.MQ;

namespace Stone.FluxoCaixaViaFila.WebApi
{
    internal class InfraContainerHelper
    {
        internal static void RegisterServices(Container c)
        {
            c.Register<ILancamentoMqFactory, LancamentoMqFactory>(Lifestyle.Singleton);
            //c.Register<IFluxoCaixaRepository, FluxoCaixaRepository>(Lifestyle.Singleton);
        }
    }
}