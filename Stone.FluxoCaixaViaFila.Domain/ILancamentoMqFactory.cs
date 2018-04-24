
namespace Stone.FluxoCaixaViaFila.Domain
{
	public interface ILancamentoMqFactory
	{
		IMessageMq Create(TipoLancamentoEnum tipoLancamento);
	}
}