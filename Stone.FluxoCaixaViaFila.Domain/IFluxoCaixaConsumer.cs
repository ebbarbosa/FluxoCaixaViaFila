using System.Collections.Generic;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public interface IFluxoCaixaConsumer
    {
        FluxoCaixa ConsolidarMes(IEnumerable<Lancamento> lancamentos);
    }
}


