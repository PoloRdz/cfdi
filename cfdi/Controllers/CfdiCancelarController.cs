using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cfdi.Controllers
{
    [Route("api/cfdi/cancelar")]
    [ApiController]
    public class CfdiCancelarController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post(CFDi cfdi)
        {
            TimbradoService service = new TimbradoService();
            var results = new Dictionary<string, Object>();
            try
            {
                cfdi = service.cancelarTimbre(cfdi);
                results.Add("cfdi", cfdi);
                return Ok(results);
            }
            catch(Exception e)
            {
                if(e is CancellationException || e is InvalidInvoiceNumberException || e is WebServiceCommunicationException)
                {
                    results.Add("message", e.Message);
                    return BadRequest(results);
                }
                results.Add("message", "Error en el servidor");
                return StatusCode(500, results);
            }
        }
    }
}
