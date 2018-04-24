using System;
using Stone.FluxoCaixaViaFila.Domain;
using Xunit;

namespace Stone.FluxoCaixaViaFila.Tests
{
    [Collection("Container")]
    public class RecebimentoMqTestFixture : ContainerTestFixture
    {
        private readonly ContainerTestFixture containerTestFixture;

        public RecebimentoMqTestFixture(ContainerTestFixture containerTestFixture)
        {
            this.containerTestFixture = containerTestFixture;
        }

        [Fact]
        public void RecebimentoMq_puts_and_gets_lancamento()
        {

            var factoryMq = containerTestFixture.Container.GetInstance<ILancamentoMqFactory>();

            var recebimento = new Lancamento(){
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

            var recebimentoGet = recMq.Get();

            Assert.Equal(recebimento.DataLancamento, recebimentoGet.DataLancamento);
            Assert.Equal(recebimento.Valor, recebimentoGet.Valor);
            Assert.Equal(recebimento.BancoDestino, recebimentoGet.BancoDestino);
        }
    }
}
