using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cfdi.Controllers
{
    [Route("api/unidad-operativa")]
    [ApiController]
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
                return BadRequest(res);
            }

        }

        // GET: api/UnidadOperativa/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UnidadOperativa
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/UnidadOperativa/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
