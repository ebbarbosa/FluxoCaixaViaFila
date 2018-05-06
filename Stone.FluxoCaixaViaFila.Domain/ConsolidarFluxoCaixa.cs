using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class ConsolidarFluxoCaixa : IConsolidarFluxoCaixa
    {
        private readonly IFluxoCaixaRepository _repository;

        public ConsolidarFluxoCaixa(IFluxoCaixaRepository fluxoCaixaRepository)
        {
            _repository = fluxoCaixaRepository;
        }

        public FluxoCaixa ConsolidarMes()
        {
            var diarios = new List<FluxoCaixaDiario>(_repository.GetDiarios());
            var consolidado = new List<FluxoCaixaDiario>();

            var porData = diarios.GroupBy(d => d.Data).Select(g => g).ToList();
            porData.Sort((d1, d2) => d1.Key.CompareTo(d2.Key));

            var dataInicio = DateTime.Today;

            for (var i = 0; i < 30; i++)
            {
                var diario = new FluxoCaixaDiario();
                diario.Data = dataInicio.AddDays(i);

                var diariosExistentes = porData.Where(f => f.Key == dataInicio.AddDays(i)).ToList();
                decimal diarioAnteriorTotal = 0m;

                if (diariosExistentes != null && diariosExistentes.Any())
                {

                    foreach (var diarioExistente in diariosExistentes)
                    {

                        diario.AddEntradas(diarioExistente.SelectMany(d => d.Entradas));
                        diario.AddSaidas(diarioExistente.SelectMany(d => d.Saidas));
                        diario.AddEncargos(diarioExistente.SelectMany(d => d.Encargos));
                    }

                    diarioAnteriorTotal = i > 0 ? consolidado.ElementAt(i - 1).Total : 0;
                    diario.PosicaoDoDia = DiarioPosicaoDoDia(diario, diarioAnteriorTotal);

                }
                else
                {
                    diarioAnteriorTotal = i > 0 ? consolidado.ElementAt(i - 1).Total : 0;
                    diario.PosicaoDoDia = DiarioPosicaoDoDia(diario, diarioAnteriorTotal);
                }

                AdicionarJuros(diario);

                consolidado.Add(diario);
            }

            return new FluxoCaixa(consolidado);
        }

        private void AdicionarJuros(FluxoCaixaDiario fluxoCaixaDiario)
        {
            if (fluxoCaixaDiario.Total < 0m)
            {
                fluxoCaixaDiario.Encargos.Add(new Registro
                {
                    Data = fluxoCaixaDiario.Data.Date,
                    Valor = CalculaJuros(fluxoCaixaDiario.Total)
                });
            }
        }

        private decimal CalculaJuros(decimal total)
        {
            return Math.Round(total * (0.83m / 100m), 2);
        }

        private static decimal DiarioPosicaoDoDia(FluxoCaixaDiario diario, decimal diarioAnteriorTotal)
        {
            return Math.Round(
                diario.Total == 0
                    ? (diarioAnteriorTotal < 0 ? 100 : (diarioAnteriorTotal == 0 ? 0 : -100))
                    : (diarioAnteriorTotal) / diario.Total * 100, 2);
        }
    }
}