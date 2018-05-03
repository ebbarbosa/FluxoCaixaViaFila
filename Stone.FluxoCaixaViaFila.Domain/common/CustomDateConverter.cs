using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class CustomDateConverter : IsoDateTimeConverter
    {
        public CustomDateConverter()
        {
            base.DateTimeFormat = "dd-MM-yyyy";
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = base.ReadJson(reader, objectType, existingValue, serializer);
            return obj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            base.WriteJson(writer, value, serializer);
        }
    }

}
