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

        public FluxoCaixa(List<FluxoCaixaDiario> consolidado)
        {
            this.consolidado = consolidado;
        }

        public int Count => consolidado.Count;

        public bool IsReadOnly => consolidado.IsReadOnly;

        public void Add(FluxoCaixaDiario item)
        {
            if (consolidado == null) consolidado = new List<FluxoCaixaDiario>();
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
            throw new NotImplementedException();
        }

        public IEnumerator<FluxoCaixaDiario> GetEnumerator()
        {
            return consolidado?.GetEnumerator();
        }

        public bool Remove(FluxoCaixaDiario item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return consolidado?.GetEnumerator();
        }
    }
}


