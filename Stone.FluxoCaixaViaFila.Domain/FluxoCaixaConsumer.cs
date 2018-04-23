using System;
using System.Collections.Generic;
using System.Linq;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class FluxoCaixaConsumer : IFluxoCaixaConsumer
    {
        public FluxoCaixa ConsolidarMes(IEnumerable<Lancamento> lancamentos)
        {

            var consolidado = new List<FluxoCaixaDiario>();

            var porData = lancamentos.GroupBy(l => l.DataLancamento).ToList();
            porData.Sort((d1, d2) => d1.Key.CompareTo(d2.Key));

            var dataInicio = porData.First().Key;
            for (var i = 0; i < 30; i++)
            {
                var diario = new FluxoCaixaDiario();
                var diarioExistente = porData.FirstOrDefault(f => f.Key == dataInicio.AddDays(i));

                if (diarioExistente != null)
                {
                    diario.Data = diarioExistente.Key;
                    diario.Entradas = diarioExistente.Where(d => d.TipoLancamento == TipoLancamentoEnum.recebimento)?
                        .Select(d => new Registro
                        {
                            Data = d.DataLancamento,
                            Valor = d.Valor
                        });

                    diario.Saidas = diarioExistente.Where(d => d.TipoLancamento == TipoLancamentoEnum.pagamento)?
                        .Select(d => new Registro
                        {
                            Data = d.DataLancamento,
                            Valor = d.Valor
                        });

                    diario.Encargos = diarioExistente
                        .Select(d => new Registro
                        {
                            Data = d.DataLancamento,
                            Valor = d.Encargos
                        });

                    var diarioAnteriorTotal = i > 0 ? consolidado.ElementAt(i - 1).Total : diario.Total;
                        diario.PosicaoDoDia = Math.Round(diario.Total == 0 ?
                        (diarioAnteriorTotal < 0 ? 100 : (diarioAnteriorTotal == 0 ? 0 : -100)) :
                                                         (diario.Total - diarioAnteriorTotal) / diario.Total * 100, 2);
                }
                else
                {
                    var diarioAnteriorTotal = i > 0 ? consolidado.ElementAt(i - 1).Total : diario.Total;
                    diario = new FluxoCaixaDiario()
                    {
                        Data = dataInicio.AddDays(i),
                        PosicaoDoDia = Math.Round(diario.Total == 0 ? 
                                             (diarioAnteriorTotal < 0 ? 100 : (diarioAnteriorTotal == 0 ? 0 : -100)) :
                                                  (diario.Total - diarioAnteriorTotal) / diario.Total * 100, 2)
                    };
                }

                consolidado.Add(diario);
            }

            return new FluxoCaixa(consolidado);
        }

    }
}