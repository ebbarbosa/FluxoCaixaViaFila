using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stone.FluxoCaixaViaFila.Domain
{
    [Serializable]
    /// <summary>
    /// Lancamento valido sera colocado na fila devida, de acordo com seu tipo_de_lancamento.
    /// Lancamento e valido quando a data_lancamento maior ou igual a data corrente.
    /// </summary>
    public class Lancamento
    {
        [JsonProperty("tipo_da_lancamento")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TipoLancamentoEnum TipoLancamento
        {
            get;
            set;
        }

        [JsonProperty("descricao")]
        public string Descricao
        {
            get;
            set;
        }

        [JsonProperty("conta_destino")]
        public string ContaDestino
        {
            get;
            set;
        }

        [JsonProperty("banco_destino")]
        public string BancoDestino
        {
            get;
            set;
        }

        [JsonProperty("tipo_conta")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TipoContaEnum TipoConta
        {
            get; set;
        }

        [JsonProperty("cpf_cnpj_destino")]
        public string CpfCnpjFormatado
        {
            get; set;
        }

        [JsonProperty("valor_do_lancamento")]
        [JsonConverter(typeof(CurrencyConverter))]
        public decimal Valor
        {
            get;
            set;
        }

        [JsonProperty("encargos")]
        [JsonConverter(typeof(CurrencyConverter))]
        public decimal Encargos
        {
            get;
            set;
        }

        [JsonProperty("data_de_lancamento")]
        [JsonConverter(typeof(CustomDateConverter), "dd-MM-yyyy")]
        public DateTime DataLancamento
        {
            get;
            set;
        }
    }
}


