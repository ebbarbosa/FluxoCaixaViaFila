using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stone.FluxoCaixaViaFila.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Stone.FluxoCaixaViaFila.WebApi.Controllers
{
    [Route("api/lancamento")]
    public class LancamentoController : Controller
    {
        private readonly ILancamentoRouter lancamentoRouter;

        public LancamentoController(ILancamentoRouter lancamentoRouter)
        {
            this.lancamentoRouter = lancamentoRouter;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Lancamento lancamento)
        {
            lancamentoRouter.RotearPraFila(lancamento);
        }

    }
}
