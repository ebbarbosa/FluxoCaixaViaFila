using System;
using System.Linq;
using Stone.FluxoCaixaViaFila.Domain;
using Xunit;

namespace Stone.FluxoCaixaViaFila.Tests
{
    public class RecebimentoMqTestFixture : ContainerTestFixture
    {
        private readonly ContainerTestFixture containerTestFixture;

        public RecebimentoMqTestFixture(ContainerTestFixture containerTestFixture)
        {
            this.containerTestFixture = containerTestFixture;
        }

        [Fact]
        public void LancamentoFactory_recebe_lancamentos_valida_e_coloca_no_repositorio()
        {

            var factoryMq = containerTestFixture.Container.GetInstance<ILancamentoMqFactory>();

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

            var recMq = factoryMq.Create(recebimento.TipoLancamento);

            recMq.Put(recebimento);

            var repo = Container.GetInstance<IFluxoCaixaRepository>();
            var fluxoCaixaDiario = repo.GetPorDia(recebimento.DataLancamento);
            Assert.NotNull(fluxoCaixaDiario);
            Assert.Equal(fluxoCaixaDiario.Total, recebimento.Valor);

            var recebimentoRegistro = fluxoCaixaDiario.Entradas.SingleOrDefault(e => e.Data.Equals(recebimento.DataLancamento));
            Assert.NotNull(recebimentoRegistro);
            Assert.Equal(recebimento.DataLancamento, recebimentoRegistro.Data);
            Assert.Equal(recebimento.Valor, recebimentoRegistro.Valor);


            Assert.NotNull(fluxoCaixaDiario.Encargos.SingleOrDefault(e => e.Data.Equals(recebimento.DataLancamento) &&
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

            var pagMq = factoryMq.Create(pagamento.TipoLancamento);

            pagMq.Put(pagamento);

            fluxoCaixaDiario = repo.GetPorDia(pagamento.DataLancamento);
            Assert.NotNull(fluxoCaixaDiario);
            Assert.Equal(fluxoCaixaDiario.Total, recebimento.Valor - pagamento.Valor);

            var registro = fluxoCaixaDiario.Saidas.SingleOrDefault(e => e.Data.Equals(pagamento.DataLancamento));
            Assert.NotNull(registro);
            Assert.Equal(pagamento.DataLancamento, registro.Data);
            Assert.Equal(pagamento.Valor, registro.Valor);

            Assert.NotNull(fluxoCaixaDiario.Encargos.SingleOrDefault(e => e.Data.Equals(pagamento.DataLancamento) &&
                                                                     e.Valor.Equals(pagamento.Valor * -1)));


        }
    }
}
