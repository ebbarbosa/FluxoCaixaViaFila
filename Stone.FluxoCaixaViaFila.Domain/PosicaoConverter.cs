using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class PosicaoConverter : JsonConverter<Decimal>
    {
        public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var valueString = reader.Value.ToString();

            NumberFormatInfo MyNFI = new NumberFormatInfo();
            MyNFI.NegativeSign = "-";
            MyNFI.CurrencyDecimalSeparator = ",";
            MyNFI.CurrencyGroupSeparator = ".";
            
            decimal converted = 0m;

            return decimal.TryParse(valueString, NumberStyles.Currency, MyNFI, out converted) ?
                          converted : throw new JsonException($"Valor {reader.Value} nao pode ser convertido, formato incorreto");
        }

        public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value:#.0}%");
        }
    }
}
