using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stone.FluxoCaixaViaFila.Domain
{
    [JsonArray]
    public class FluxoCaixa : ICollection<FluxoCaixaDiario>
    {
        private ICollection<FluxoCaixaDiario> consolidado;
        private IEnumerable<KeyValuePair<DateTime, FluxoCaixaDiario>> copyOfLancamento;

        public FluxoCaixa(IEnumerable<FluxoCaixaDiario> consolidado)
        {
            this.consolidado = new List<FluxoCaixaDiario>(consolidado);
            if (consolidado == null) consolidado = new List<FluxoCaixaDiario>();
        }

        public int Count => consolidado.Count;

        public bool IsReadOnly => consolidado.IsReadOnly;

        public void Add(FluxoCaixaDiario item)
        {
            consolidado.Add(item);
        }

        public void Clear()
        {
            consolidado?.Clear();
        }

        public bool Contains(FluxoCaixaDiario item)
        {
            return consolidado.Contains(item);
        }

        public void CopyTo(FluxoCaixaDiario[] array, int arrayIndex)
        {
            consolidado?.CopyTo(array, arrayIndex);
        }

        public IEnumerator<FluxoCaixaDiario> GetEnumerator()
        {
            return consolidado?.GetEnumerator();
        }

        public bool Remove(FluxoCaixaDiario item)
        {
            return consolidado.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return consolidado?.GetEnumerator();
        }
    }
}


