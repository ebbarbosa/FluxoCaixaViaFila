using System;
using Stone.FluxoCaixaViaFila.Domain;
using Xunit;

namespace Stone.FluxoCaixaViaFila.Tests
{
    [Collection("Container")]
    public class LancamentoFactoryTestFixture : ContainerTestFixture
    {
        ContainerTestFixture _containerTestFixture;
        private DateTime _dataLancamento;
        private string _lancamentoJson;

        public LancamentoFactoryTestFixture(ContainerTestFixture containerTestFixture)
        {
            _containerTestFixture = containerTestFixture;

            _dataLancamento = DateTime.Now;

            _lancamentoJson = @"{
""tipo_da_lancamento"":""pagamento"",
""descricao"": ""Qualquer descrição sobre o pagamento"",
""conta_destino"": ""Conta do destinatario"",
""banco_destino"": ""Banco do destinatario"",
""tipo_de_conta"": ""corrente"",
""cpf_cnpj_destino"": ""000.000.000.00"",
""valor_do_lancamento"": ""R$1.000,00"",
""encargos"": ""R$0.000,10"",
""data_de_lancamento"": """ + _dataLancamento.ToString("dd-MM-yyyy") + "\"}";

        }


        [Fact]
        public void LancamentoFactory_can_deserialize_a_json_file()
        {
            var lctoFactory = _containerTestFixture.Container.GetInstance<ILancamentoFactory>();
            var lancamento = lctoFactory.Create(_lancamentoJson);
            Assert.IsType<Lancamento>(lancamento);
            Assert.Equal(TipoContaEnum.corrente, lancamento.TipoConta);
            Assert.Equal("Qualquer descrição sobre o pagamento", lancamento.Descricao);
                         Assert.Equal(TipoLancamentoEnum.pagamento, lancamento.TipoLancamento);
            Assert.Equal("Banco do destinatario", lancamento.BancoDestino);
            Assert.Equal("Conta do destinatario", lancamento.ContaDestino);
            Assert.Equal("000.000.000.00", lancamento.CpfCnpjFormatado);
            Assert.Equal(1000m, lancamento.Valor);
            Assert.Equal(.10m, lancamento.Encargos);
            Assert.Equal(_dataLancamento.Date, lancamento.DataLancamento);
        }
    }
}
