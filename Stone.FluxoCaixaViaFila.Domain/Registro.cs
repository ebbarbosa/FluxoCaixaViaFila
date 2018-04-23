using System;
using Newtonsoft.Json;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class Registro
    {
        [JsonProperty("data")]
        public DateTime Data { get; set; }
        
        [JsonProperty("valor")]
        [JsonConverter(typeof(CurrencyConverter))]
        public decimal Valor { get; set; }
    }
}


