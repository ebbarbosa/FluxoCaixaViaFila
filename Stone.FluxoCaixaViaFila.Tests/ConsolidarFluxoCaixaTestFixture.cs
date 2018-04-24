using Stone.FluxoCaixaViaFila.Domain;
using Xunit;
using System;
using System.Linq;

namespace Stone.FluxoCaixaViaFila.Tests
{
    [Collection("Container")]
    public class ConsolidarFluxoCaixaTestFixture : ContainerTestFixture
    {
        ContainerTestFixture _containerTestFixture;
        private DateTime dia1 = DateTime.Today;

        public ConsolidarFluxoCaixaTestFixture(ContainerTestFixture containerTestFixture)
        {
            _containerTestFixture = containerTestFixture;
        }

        [Fact]
        public void Dados_lancamentos_consumer_cria_fluxo_caixa_mes()
        {
            //Given
            var lancamentos = new[]{
                new Lancamento(){
                    TipoConta = TipoContaEnum.corrente,
                    DataLancamento = dia1,
                    Valor = 1000.00m,
                    TipoLancamento = TipoLancamentoEnum.pagamento
                },
                new Lancamento(){
                    TipoConta = TipoContaEnum.corrente,
                    DataLancamento = dia1,
                    Valor = 2000.00m,
                    TipoLancamento = TipoLancamentoEnum.recebimento
                },
                new Lancamento(){
                    TipoConta = TipoContaEnum.poupanca,
                    DataLancamento = dia1.AddDays(1),
                    Valor = 1100.00m,
                    TipoLancamento = TipoLancamentoEnum.pagamento
                },
                new Lancamento(){
                    TipoConta = TipoContaEnum.corrente,
                    DataLancamento = dia1.AddDays(1),
                    Valor = 2200.00m,
                    TipoLancamento = TipoLancamentoEnum.recebimento
                },
            };

            //When
            var fluxo = _containerTestFixture.Container.GetInstance<IConsolidarFluxoCaixa>();
            var consolidadoMes = fluxo.ConsolidarMes(lancamentos);

            //Then
            Assert.True(consolidadoMes.Count == 30);
            var primeiroDia = consolidadoMes.First();
            var segundoDia = consolidadoMes.First(d => d.Data == dia1.AddDays(1));

            Assert.NotNull(primeiroDia);
            Assert.NotNull(segundoDia);
            Assert.Equal(dia1, primeiroDia.Data);
            Assert.Equal(0m, primeiroDia.PosicaoDoDia);
            Assert.Equal(1000m, primeiroDia.Total);
            Assert.Equal(9.09m, segundoDia.PosicaoDoDia);
            Assert.Equal(1100m, segundoDia.Total);
            FluxoCaixaDiario ultimoDia = consolidadoMes.Last();
            Assert.Equal(dia1.AddDays(29), ultimoDia.Data);
            Assert.Equal(0m, ultimoDia.PosicaoDoDia);

        }
    }
}
