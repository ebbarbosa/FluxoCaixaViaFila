using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Stone.FluxoCaixaViaFila.Domain
{
    public class FluxoCaixaDiario
    {
        private List<Registro> _entradas;
        private List<Registro> _saidas;
        private List<Registro> _encargos;


        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("entradas")]
        public IList<Registro> Entradas
        {
            get
            {
                return _entradas ?? (_entradas = new List<Registro>());
            }
            set
            {
                Entradas = new List<Registro>(value);
            }
        }

        [JsonProperty("saidas")]
        public IList<Registro> Saidas
        {
            get
            {
                return _saidas ?? (_saidas = new List<Registro>());
            }
            set
            {
                Saidas = new List<Registro>(value);
            }
        }

        [JsonProperty("encargos")]
        public IList<Registro> Encargos
        {
            get
            {
                return _encargos ?? (_encargos = new List<Registro>());
            }
            set
            {
                Encargos = new List<Registro>(value);
            }
        }

        [JsonProperty("total")]
        public decimal Total
        {
            get
            {
                var sumEntradas = this.Entradas?.Sum(e => e.Valor);
                var sumSaidas = this.Saidas?.Sum(e => e.Valor);
                var sumEncargos = this.Encargos?.Sum(e => e.Valor);

                return sumEntradas.GetValueOrDefault()
                - sumSaidas.GetValueOrDefault()
                - sumEncargos.GetValueOrDefault();
            }
        }


        public void Add(Lancamento lancamento)
        {
            if (lancamento.TipoLancamento == TipoLancamentoEnum.pagamento)
            {
                this.Saidas.ToList().Add(new Registro
                {
                    Data = lancamento.DataLancamento,
                    Valor = lancamento.Valor
                });
                if (lancamento.Encargos > 0)
                {
                    this.Encargos.ToList().Add(new Registro
                    {
                        Data = lancamento.DataLancamento,
                        Valor = lancamento.Encargos * -1 //negativo porque sao pagamentos
                    });
                }
            }
            else
            {
                this.Entradas.ToList().Add(new Registro
                {
                    Data = lancamento.DataLancamento,
                    Valor = lancamento.Valor
                });
                if (lancamento.Encargos > 0)
                {
                    this.Encargos.ToList().Add(new Registro
                    {
                        Data = lancamento.DataLancamento,
                        Valor = lancamento.Encargos
                    });
                }
            }
        }

        [JsonProperty("posicao_do_dia")]
        [JsonConverter(typeof(PosicaoConverter))]
        public decimal PosicaoDoDia { get; set; }

        public void AddEncargos(IEnumerable<Registro> fluxodiarioEncargos)
        {
            foreach (var fluxodiarioEncargo in fluxodiarioEncargos)
            {
                this.Encargos.Add(fluxodiarioEncargo);
            }
        }

        public void AddSaidas(IEnumerable<Registro> fluxodiarioSaidas)
        {
            foreach (var fluxodiarioEncargo in fluxodiarioSaidas)
            {
                this.Saidas.Add(fluxodiarioEncargo);
            }
        }

        public void AddEntradas(IEnumerable<Registro> fluxodiarioEntradas)
        {
            foreach (var fluxodiarioEncargo in fluxodiarioEntradas)
            {
                this.Entradas.Add(fluxodiarioEncargo);
            }
        }
    }
}