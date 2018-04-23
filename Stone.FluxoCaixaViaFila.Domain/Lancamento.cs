using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class Lancamento
    {
        [JsonProperty("tipo_da_lancamento")]
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
        public DateTime DataLancamento
        {
            get;
            set;
        }
    }
}


