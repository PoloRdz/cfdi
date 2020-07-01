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
    [Route("api/[controller]")]
    [ApiController]
    public class CfdiCancelarController : ControllerBase
    {
        // GET: api/CfdiCancelar
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/CfdiCancelar/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/CfdiCancelar
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
                return BadRequest(results);
            }
        }

        // PUT: api/CfdiCancelar/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
