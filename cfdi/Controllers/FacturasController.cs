using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FacturasController : ControllerBase
    {
        // GET: api/Facturas
        [HttpGet]
        public IActionResult Get(int pagina, int rpp)
        {
            var res = new Dictionary<string, Object>();
            var serv = new CFDiService();
            try
            {
                res = serv.GetCFDis(pagina, rpp);
                return Ok(res);
            } catch(Exception e)
            {
                if(e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                res.Add("message", "Error en el servidor");
                return BadRequest(res);
            }            
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Facturas
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
