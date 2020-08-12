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

namespace cfdi.Controllers
{
    [Route("api/unidad-operativa")]
    [ApiController]
    [Authorize]
    public class UnidadOperativaController : ControllerBase
    {
        // GET: api/UnidadOperativa
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UnidadOperativa
        [HttpGet("zona/{id}")]
        public IActionResult GetPorZona(int id)
        {
            var res = new Dictionary<string, object>();
            try
            {
                UnidadOperativaService uoSrv = new UnidadOperativaService();
                res = uoSrv.getUnidadesOperativasPorZona(id);
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

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN, UOP")]
        public IActionResult Get(int id)
        {
            UnidadOperativaService unidadOperativaService = new UnidadOperativaService();
            var res = new Dictionary<string, Object>();
            try
            {
                res.Add("unidadOperativa", unidadOperativaService.ObtenerUnidadOperativa(id));
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
        [Authorize(Roles = "ADMIN, UOP")]
        public IActionResult Post(UnidadOperativa unidadOperativa)
        {
            UnidadOperativaService unidadOperativaService = new UnidadOperativaService();
            try
            {
                unidadOperativaService.InsertarUnidadOperativa(unidadOperativa);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // PUT: api/Zona/5
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN, UOP")]
        public IActionResult Put(int id, UnidadOperativa unidadOperativa)
        {
            UnidadOperativaService unidadOperativaService = new UnidadOperativaService();
            try
            {
                unidadOperativaService.ActualizarUnidadOperativa(unidadOperativa);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // DELETE: api/Zona/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN, UOP")]
        public IActionResult Delete(int id)
        {
            UnidadOperativaService unidadOperativaService = new UnidadOperativaService();
            try
            {
                unidadOperativaService.EliminarUnidadOperativa(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // Put: api/Zona/activar/5
        [HttpPut("activar/{id}")]
        [Authorize(Roles = "ADMIN, UOP")]
        public IActionResult PutActivar(int id)
        {
            UnidadOperativaService unidadOperativaService = new UnidadOperativaService();
            try
            {
                unidadOperativaService.ActivarUnidadOperativa(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }
    }
}
