namespace Stone.FluxoCaixaViaFila.Domain
{
    public class LancamentoRouter : ILancamentoRouter
    {
        private ILancamentoSpecificationFactory lancamentoSpecificationFactory;
        private ILancamentoMqFactory lancamentoMqFactory;

        public LancamentoRouter(ILancamentoSpecificationFactory lancamentoSpecificationFactory, ILancamentoMqFactory lancamentoMqFactory)
        {
            this.lancamentoMqFactory = lancamentoMqFactory;
            this.lancamentoSpecificationFactory = lancamentoSpecificationFactory;
        }

        public void RotearPraFila(Lancamento lancamento)
        {
            var lancamentoSpecification = lancamentoSpecificationFactory.Create(lancamento);
            lancamentoSpecification.Validate();

            var lancamentoMq = lancamentoMqFactory.Create(lancamento.TipoLancamento);
            lancamentoMq.Put(lancamento);
        }
    }
}