using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class FluxoCaixaDiario
    {

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("entradas")]
        public IEnumerable<Registro> Entradas { get; set; }

        [JsonProperty("saidas")]
        public IEnumerable<Registro> Saidas { get; set; }

        [JsonProperty("encargos")]
        public IEnumerable<Registro> Encargos { get; set; }

        [JsonProperty("total")]
        public decimal Total
        {
            get
            {
                var sumEntradas = this.Entradas?.Sum(e => e.Valor);
                var sumSaidas = this.Saidas?.Sum(e => e.Valor);
                var sumEncargos = this.Encargos?.Sum(e => e.Valor);

                return sumEntradas.GetValueOrDefault()
                - sumSaidas.GetValueOrDefault()
                - sumEncargos.GetValueOrDefault();
            }
        }

        [JsonProperty("posicao_do_dia")]
        [JsonConverter(typeof(PosicaoConverter))]
        public decimal PosicaoDoDia { get; set; }
    }
}