using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stone.FluxoCaixaViaFila.Domain
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TipoLancamentoEnum
    {
        pagamento,
        recebimento
    }
}