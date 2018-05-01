using Stone.FluxoCaixaViaFila.Domain;
using System.Collections.Generic;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class FluxoCaixaRepository : IFluxoCaixaRepository
    {
        private readonly IList<FluxoCaixaDiario> _fluxosDiario = new List<FluxoCaixaDiario>();

        public void Add(FluxoCaixaDiario fluxoCaixaDiario)
        {
            _fluxosDiario.Add(fluxoCaixaDiario);
        }

        public IEnumerable<FluxoCaixaDiario> GetDiarios()
        {
            return _fluxosDiario;
        }
    }
}