using System;
using System.Linq;
using System.Threading;
using Stone.FluxoCaixaViaFila.Domain;
using Xunit;

namespace Stone.FluxoCaixaViaFila.Tests
{

    [Collection("Container")]
    public class DomainTestFixture : ContainerTestFixture
    {
        private ContainerTestFixture _containerTestFixture;

        public DomainTestFixture(ContainerTestFixture containerTestFixture)
        {
            this._containerTestFixture = containerTestFixture;
        }

        [Fact]
        public void LancamentoFactory_recebe_lancamentos_valida_e_coloca_no_repositorio()
        {

            var lancamentoSpecificationFactory = _containerTestFixture.Container.GetInstance<ILancamentoSpecificationFactory>();

            var recebimento = new Lancamento()
            {
                DataLancamento = DateTime.Now.Date,
                Valor = 1000m,
                TipoLancamento = TipoLancamentoEnum.recebimento,
                TipoConta = TipoContaEnum.corrente,
                BancoDestino = "237",
                ContaDestino = "00021212",
                CpfCnpjFormatado = "11.111.111/0001-11",
                Descricao = "rec put and get",
                Encargos = 0.44m,
            };

            var lancamentoSpecification = lancamentoSpecificationFactory.Create(recebimento);

            lancamentoSpecification.Validate();

            var fluxoDiario = new FluxoCaixaDiario();
            fluxoDiario.Add(recebimento);

            var repositorioFluxoCaixa = Container.GetInstance<IFluxoCaixaRepository>();
            repositorioFluxoCaixa.Add(fluxoDiario);

            var consolidarCaixa = Container.GetInstance<IConsolidarFluxoCaixa>();

            var recebimentoRegistro = fluxoDiario.Entradas.SingleOrDefault(e => e.Data.Equals(recebimento.DataLancamento));
            Assert.NotNull(recebimentoRegistro);
            Assert.Equal(recebimento.DataLancamento, recebimentoRegistro.Data);
            Assert.Equal(recebimento.Valor, recebimentoRegistro.Valor);
            Assert.NotNull(fluxoDiario.Encargos.SingleOrDefault(e => e.Data.Equals(recebimento.DataLancamento) &&
                                                                     e.Valor.Equals(recebimento.Valor)));
                           
            var pagamento = new Lancamento()
            {
                DataLancamento = DateTime.Now.Date,
                Valor = 1000m,
                TipoLancamento = TipoLancamentoEnum.pagamento,
                TipoConta = TipoContaEnum.corrente,
                BancoDestino = "237",
                ContaDestino = "00021212",
                CpfCnpjFormatado = "11.111.111/0001-11",
                Descricao = "rec put and get",
                Encargos = 0.44m,
            };

            var pagamentoSpecification = lancamentoSpecificationFactory.Create(pagamento);
            pagamentoSpecification.Validate();

            var fluxoDiarioPg = new FluxoCaixaDiario();
            fluxoDiarioPg.Add(pagamento);

            repositorioFluxoCaixa.Add(fluxoDiarioPg);
            
            Assert.NotNull(fluxoDiarioPg);
            Assert.Equal(fluxoDiarioPg.Total, recebimento.Valor - pagamento.Valor);

            var registro = fluxoDiarioPg.Saidas.SingleOrDefault(e => e.Data.Equals(pagamento.DataLancamento));
            Assert.NotNull(registro);
            Assert.Equal(pagamento.DataLancamento, registro.Data);
            Assert.Equal(pagamento.Valor, registro.Valor);

            Assert.NotNull(fluxoDiarioPg.Encargos.SingleOrDefault(e => e.Data.Equals(pagamento.DataLancamento) &&
                                                                     e.Valor.Equals(pagamento.Valor * -1)));



            var consolidado = consolidarCaixa.ConsolidarMes();

            Assert.NotNull(consolidado);
            Assert.Equal(30, consolidado.Count);
        }
    }
}
