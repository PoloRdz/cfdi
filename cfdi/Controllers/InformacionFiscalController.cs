using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InformacionFiscalController : ControllerBase
    {
        // GET: api/<InformacionFiscalController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<InformacionFiscalController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var res = new Dictionary<string, Object>();
            var uService = new UsuarioService();
            try
            {
                res.Add("informacionFiscal", uService.getInformacionFiscal(id));
                return Ok(res);
            } 
            catch(Exception e)
            {
                if(e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // POST api/<InformacionFiscalController>/5 (id usuario)
        [HttpPost("{id}")]
        public IActionResult Post(int id, InformacionFiscal informacionFiscal)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService uServ = new UsuarioService();
            try
            {
                res.Add("informacionFiscal", uServ.insertInformacionFiscal(id, informacionFiscal));
                return Ok(res);
            }
            catch (Exception e)
            { 
                if(e is DuplicateRFCException)
                {
                    res.Add("message", e.Message);
                    return Conflict(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // PUT api/<InformacionFiscalController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InformacionFiscalController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
