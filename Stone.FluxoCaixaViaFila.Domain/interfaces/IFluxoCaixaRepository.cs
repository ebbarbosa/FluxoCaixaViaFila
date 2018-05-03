using System;
using System.Collections.Generic;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public interface IFluxoCaixaRepository
    {
        void Add(FluxoCaixaDiario fluxoCaixaDiario);
        IEnumerable<FluxoCaixaDiario> GetDiarios();

    }
}