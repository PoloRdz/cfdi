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
    public class FacturaController : ControllerBase
    {
        // GET: api/Factura
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
                return StatusCode(500, res);
            }            
        }

        // GET: api/Factura/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var res = new Dictionary<string, Object>();
            var serv = new CFDiService();
            try
            {
                res = serv.GetFactura(id);
                return Ok(res);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // POST: api/Factura
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // GET: api/Factura/mis-facturas/1
        [HttpGet("mis-facturas/{id}")]
        public IActionResult GetMisFacturas(int id, int pagina, int rpp)
        {
            var res = new Dictionary<string, Object>();
            var serv = new CFDiService();
            try
            {
                res = serv.GetMisFacturas(id, pagina, rpp);
                return Ok(res);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }
    }
}
