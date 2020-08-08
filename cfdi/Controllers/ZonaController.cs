using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ZonaController : ControllerBase
    {
        // GET: api/Zona/lista
        [HttpGet("lista")]
        public IActionResult GetLista()
        {
            ZonaService zser = new ZonaService();
            var res = new Dictionary<string, Object>();
            try
            {
                res = zser.ObtenerZonasLista();
                return Ok(res);
            }
            catch (Exception e)
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

        // GET: api/Zona
        [HttpGet]
        [Authorize(Roles = "ADMIN, ZONA")]
        public IActionResult Get(int pagina, int rpp)
        {
            ZonaService zser = new ZonaService();
            var res = new Dictionary<string, Object>();
            try
            {
                res = zser.ObtenerZonas(pagina, rpp);
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

        // GET: api/Zona/5
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN, ZONA")]
        public IActionResult Get(int id)
        {
            ZonaService zser = new ZonaService();
            var res = new Dictionary<string, Object>();
            try
            {
                res.Add("zona", zser.ObtenerZona(id));
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
        [Authorize(Roles = "ADMIN, ZONA")]
        public IActionResult Post(Zona zona)
        {
            ZonaService zser = new ZonaService();
            try
            {
                zser.InsertarZona(zona);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // PUT: api/Zona/5
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN, ZONA")]
        public IActionResult Put(int id, Zona zona)
        {
            ZonaService zser = new ZonaService();
            try
            {
                zser.ActualizarZona(zona);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // DELETE: api/Zona/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN, ZONA")]
        public IActionResult Delete(int id)
        {
            ZonaService zser = new ZonaService();
            try
            {
                zser.EliminarZona(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // Put: api/Zona/activar/5
        [HttpPut("activar/{id}")]
        [Authorize(Roles = "ADMIN, ZONA")]
        public IActionResult PutActivar(int id)
        {
            ZonaService zser = new ZonaService();
            try
            {
                zser.ActivarZona(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }
    }
}
