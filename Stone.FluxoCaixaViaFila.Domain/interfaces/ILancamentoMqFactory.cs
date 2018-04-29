
namespace Stone.FluxoCaixaViaFila.Domain
{
	public interface ILancamentoMqFactory
	{
		IPublisherMq Create(TipoLancamentoEnum tipoLancamento);
	}
}