
using Microsoft.Extensions.Hosting;

namespace Stone.FluxoCaixaViaFila.Domain
{
	public interface IFluxoCaixaConsumer
	{
		void Execute();
    }
}