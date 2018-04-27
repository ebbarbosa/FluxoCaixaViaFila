using System;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public interface IFluxoCaixaRepository
    {
        FluxoCaixa Get();
        FluxoCaixa Get(DateTime dataInicio);
        FluxoCaixaDiario GetPorDia(DateTime dataLancamento);
    }
}