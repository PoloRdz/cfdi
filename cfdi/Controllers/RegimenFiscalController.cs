using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegimenFiscalController : ControllerBase
    {
        // GET: api/<RegimenFiscalController>
        [HttpGet("lista")]
        public IActionResult GetLista()
        {
            var res = new Dictionary<string, Object>();
            var rfServ = new RegimenFiscalService();
            try
            {
                res = rfServ.GetRegimenesFiscales();
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

        [HttpGet()]
        [Authorize(Roles = "ADMIN, REGFI")]
        public IActionResult Get(int pagina, int rpp)
        {
            RegimenFiscalService regimenFiscalService = new RegimenFiscalService();
            var res = new Dictionary<string, Object>();
            try
            {
                res = regimenFiscalService.ObtenerRegimenesFiscales(pagina, rpp);
                return Ok(res);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(new { Message = e.Message });
                }
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN, REGFI")]
        public IActionResult Get(int id)
        {
            RegimenFiscalService regimenFiscalService = new RegimenFiscalService();
            var res = new Dictionary<string, Object>();
            try
            {
                res.Add("regimenFiscal", regimenFiscalService.ObtenerRegimenFiscal(id));
                return Ok(res);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(new { Message = e.Message });
                }
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // POST: api/Zona
        [HttpPost]
        [Authorize(Roles = "ADMIN, REGFI")]
        public IActionResult Post(RegimenFiscal regimenFiscal)
        {
            RegimenFiscalService regimenFiscalService = new RegimenFiscalService();
            try
            {
                regimenFiscalService.InsertarRegimenFiscal(regimenFiscal);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // PUT: api/Zona/5
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN, REGFI")]
        public IActionResult Put(int id, RegimenFiscal regimenFiscal)
        {
            RegimenFiscalService regimenFiscalService = new RegimenFiscalService();
            try
            {
                regimenFiscalService.ActualizarRegimenFiscal(regimenFiscal);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // DELETE: api/Zona/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN, REGFI")]
        public IActionResult Delete(int id)
        {
            RegimenFiscalService regimenFiscalService = new RegimenFiscalService();
            try
            {
                regimenFiscalService.EliminarRegimenFiscal(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // Put: api/Zona/activar/5
        [HttpPut("activar/{id}")]
        [Authorize(Roles = "ADMIN, REGFI")]
        public IActionResult PutActivar(int id)
        {
            RegimenFiscalService regimenFiscalService = new RegimenFiscalService();
            try
            {
                regimenFiscalService.ActivarRegimenFiscal(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }
    }
}
