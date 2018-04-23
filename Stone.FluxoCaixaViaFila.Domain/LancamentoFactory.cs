using Newtonsoft.Json;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class LancamentoFactory : ILancamentoFactory
    {
        public Lancamento Create(string jsonLancamento)
        {
            var lancamento = JsonConvert.DeserializeObject<Lancamento>(
                jsonLancamento, new JsonSerializerSettings
                {
                    DateFormatString = "dd-MM-yyyy",
                    Culture = new System.Globalization.CultureInfo("pt-br")
                });
            return lancamento;
        }
    }
}
