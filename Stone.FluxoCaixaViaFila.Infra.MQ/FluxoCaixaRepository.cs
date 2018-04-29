using System;
using System.Linq;
using Stone.FluxoCaixaViaFila.Domain;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class FluxoCaixaRepository : IFluxoCaixaRepository
    {
        private IDictionary<DateTime, FluxoCaixaDiario> _lancamentos = new Dictionary<DateTime, FluxoCaixaDiario>();

        public void Add(string message)
        {
            var lancamento = JsonConvert.DeserializeObject<Lancamento>(message);

            if (_lancamentos.ContainsKey(lancamento.DataLancamento.Date))
            {
                _lancamentos[lancamento.DataLancamento.Date].Add(lancamento);
            }
            else
            {

                var novoFluxoCaixaDiario = new FluxoCaixaDiario();
                novoFluxoCaixaDiario.Add(lancamento);
                _lancamentos.Add(lancamento.DataLancamento, novoFluxoCaixaDiario);
            }
        }

        public FluxoCaixa Get()
        {
            return Get(DateTime.Today);
        }

        public FluxoCaixa Get(DateTime dataInicio)
        {
            var copyOfLancamento = _lancamentos.Where(l => l.Key >= dataInicio && l.Key < dataInicio.AddDays(30))
                                               .Select(l => l.Value)
                                               .AsEnumerable();

            return new FluxoCaixa(copyOfLancamento);
        }

        public FluxoCaixaDiario GetPorDia(DateTime dataLancamento)
        {
            return _lancamentos[dataLancamento.Date] ?? new FluxoCaixaDiarioVazio();
        }
    }
}