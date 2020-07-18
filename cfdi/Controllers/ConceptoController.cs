using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConceptoController : ControllerBase
    {

        // GET api/<ConceptoController>/5
        [HttpGet("factura/{id}")]
        public IActionResult GetConceptosFacturas(int id)
        {
            var res = new Dictionary<string, Object>();
            var conceptoServce = new ConceptoService();
            try
            {
                res = conceptoServce.GetConceptosFactura(id);
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
