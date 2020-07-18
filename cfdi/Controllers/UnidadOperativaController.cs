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
        public IActionResult GetFromZona(int id)
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
    }
}
