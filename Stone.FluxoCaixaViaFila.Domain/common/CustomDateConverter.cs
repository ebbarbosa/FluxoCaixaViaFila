using Newtonsoft.Json.Converters;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class CustomDateConverter : IsoDateTimeConverter
    {
        public CustomDateConverter(string format)
        {
            base.DateTimeFormat = format;
        }
    }

}
