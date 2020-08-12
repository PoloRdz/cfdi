using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsoCFDiController : ControllerBase
    {
        // GET: api/<UsoCFDiController>
        [HttpGet("lista")]
        public IActionResult Get()
        {
            UsoCFDiService ser = new UsoCFDiService();
            var res = new Dictionary<string, Object>();
            try
            {
                res = ser.getUsoCFDis();
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
        [Authorize(Roles = "ADMIN, UCFDI")]
        public IActionResult Get(int pagina, int rpp)
        {
            UsoCFDiService usoCFDIService = new UsoCFDiService();
            var res = new Dictionary<string, Object>();
            try
            {
                res = usoCFDIService.ObtenerUsoCFDis(pagina, rpp);
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
        [Authorize(Roles = "ADMIN, UCFDI")]
        public IActionResult Get(string id)
        {
            UsoCFDiService usoCFDIService = new UsoCFDiService();
            var res = new Dictionary<string, Object>();
            try
            {
                res.Add("usoCFDi", usoCFDIService.ObtenerUsoCFDi(id));
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
        [Authorize(Roles = "ADMIN, UCFDI")]
        public IActionResult Post(UsoCFDi usoCFDi)
        {
            UsoCFDiService usoCFDIService = new UsoCFDiService();
            try
            {
                usoCFDIService.InsertarUsoCFDi(usoCFDi);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // PUT: api/Zona/5
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN, UCFDI")]
        public IActionResult Put(string id, UsoCFDi usoCFDi)
        {
            UsoCFDiService usoCFDIService = new UsoCFDiService();
            try
            {
                usoCFDIService.ActualizarUsoCFDi(usoCFDi);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // DELETE: api/Zona/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN, UCFDI")]
        public IActionResult Delete(string id)
        {
            UsoCFDiService usoCFDIService = new UsoCFDiService(); 
            try
            {
                usoCFDIService.EliminarUsoCFDi(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // Put: api/Zona/activar/5
        [HttpPut("activar/{id}")]
        [Authorize(Roles = "ADMIN, UCFDI")]
        public IActionResult PutActivar(string id)
        {
            UsoCFDiService usoCFDIService = new UsoCFDiService();
            try
            {
                usoCFDIService.ActivarUsoCFDi(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }
    }
}
