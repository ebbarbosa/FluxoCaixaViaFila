using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class FluxoCaixaDiarioVazio : FluxoCaixaDiario
    {
        public FluxoCaixaDiarioVazio()
        {
            this.Entradas = new Registro[] { };
            this.Saidas = new Registro[] { };
            this.Encargos = new Registro[] { };
        }
    }
}