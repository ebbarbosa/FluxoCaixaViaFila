using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class CurrencyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var valueString = reader.Value.ToString();

            NumberFormatInfo MyNFI = new NumberFormatInfo();
            MyNFI.NegativeSign = "-";
            MyNFI.CurrencyDecimalSeparator = ",";
            MyNFI.CurrencyGroupSeparator = ".";
            MyNFI.CurrencySymbol = "R$";

            decimal converted = 0m;

            return decimal.TryParse(valueString, NumberStyles.Currency, MyNFI, out converted) ?
                          converted : throw new JsonException($"Valor {reader.Value} nao pode ser convertido, formato incorreto");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueNumber = 0m;
            valueNumber = decimal.TryParse(value.ToString(), out valueNumber) ? valueNumber : 0m;
            writer.WriteValue($"R$ {valueNumber:###.##0,00}");
        }

    }
}
