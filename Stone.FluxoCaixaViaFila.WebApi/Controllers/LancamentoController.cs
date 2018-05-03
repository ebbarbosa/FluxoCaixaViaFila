using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using Stone.FluxoCaixaViaFila.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Stone.FluxoCaixaViaFila.WebApi.Controllers
{
    /// <summary>
    /// Lancamento controller.
    /// </summary>
    [Route("api/lancamento")]
    public class LancamentoController : Controller
    {
        private readonly ILancamentoRouter lancamentoRouter;

        public LancamentoController(ILancamentoRouter lancamentoRouter)
        {
            this.lancamentoRouter = lancamentoRouter;
        }

        /// <summary>
        /// Envia o lancamento para ser validado e processado por sua devida fila.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="lancamento">Lancamento.</param>
        // POST api/lancamento
        [HttpPost]
        [Produces("application/json", Type = typeof(Lancamento))]
        public IActionResult Post([FromBody] Lancamento lancamento)
        {
            try
            {
                lancamentoRouter.RotearPraFila(lancamento);
                return Json(lancamento);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e);
            }
        }

    }
}
