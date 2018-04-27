using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stone.FluxoCaixaViaFila.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Stone.FluxoCaixaViaFila.WebApi.Controllers
{
    /// <summary>
    /// Fluxo caixa controller.
    /// </summary>
    [Route("api/fluxocaixa")]
    public class FluxoCaixaController : Controller
    {
        private readonly IFluxoCaixaRepository fluxoCaixaRepository;

        public FluxoCaixaController(IFluxoCaixaRepository fluxoCaixaRepository)
        {
            this.fluxoCaixaRepository = fluxoCaixaRepository;
        }

        /// <summary>
        /// Obter o fluxo de caixa de 30 dias a partir da data corrente.
        /// </summary>
        /// <returns>The get.</returns>
        // GET: api/fluxocaixa
        [HttpGet]
        public FluxoCaixa Get()
        {
            return fluxoCaixaRepository.Get();
        }

    }
}
