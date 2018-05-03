using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class CurrencyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal) || objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var valueString = reader.Value.ToString();

            var MyNFI = NumberFormatInfo();

            decimal converted = 0m;

            return decimal.TryParse(valueString, NumberStyles.Currency, MyNFI, out converted) ?
                          converted : throw new JsonException($"Valor {reader.Value} nao pode ser convertido, formato incorreto");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var MyNFI = NumberFormatInfo();
            var valueNumber = 0m;
            valueNumber = decimal.TryParse(value.ToString(), out valueNumber) ? valueNumber : 0m;
            writer.WriteValue(valueNumber.ToString(MyNFI));
        }

        private static NumberFormatInfo NumberFormatInfo()
        {
            NumberFormatInfo MyNFI = new NumberFormatInfo();
            MyNFI.NegativeSign = "-";
            MyNFI.CurrencyDecimalSeparator = ",";
            MyNFI.CurrencyGroupSeparator = ".";
            MyNFI.CurrencyDecimalDigits = 2;
            MyNFI.CurrencySymbol = "R$";
            return MyNFI;
        }

    }
}
