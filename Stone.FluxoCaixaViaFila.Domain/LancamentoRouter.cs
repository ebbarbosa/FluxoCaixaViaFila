namespace Stone.FluxoCaixaViaFila.Domain
{
    public class LancamentoRouter : ILancamentoRouter
    {
        private ILancamentoSpecification _lancamentoSpecification;
        private ILancamentoMqFactory _lancamentoMqFactory;

        public LancamentoRouter(ILancamentoSpecification lancamentoSpecification, ILancamentoMqFactory lancamentoMqFactory)
        {
            _lancamentoMqFactory = lancamentoMqFactory;
            _lancamentoSpecification = lancamentoSpecification;
        }

        public void RotearPraFila(Lancamento lancamento)
        {
            _lancamentoSpecification.Validate(lancamento);
            var lancamentoMq = _lancamentoMqFactory.Create(lancamento.TipoLancamento);
            lancamentoMq.Put(lancamento);
        }
    }
}