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
            try
            {
                cfdi = service.cancelarTimbre(cfdi);
                return Ok(cfdi);
            }
            catch(CancellationException e)
            {
                return BadRequest(e.Message);
            }
            catch(InvalidInvoiceStatusException e)
            {
                return BadRequest(e.Message);
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
