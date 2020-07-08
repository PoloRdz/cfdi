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
    public class ZonaController : ControllerBase
    {
        // GET: api/Zona
        [HttpGet]
        public IActionResult Get()
        {
            ZonaService zser = new ZonaService();
            var res = new Dictionary<string, Object>();
            try
            {
                res = zser.getZonas();
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
                return BadRequest(res);
            }
            
        }

        //// GET: api/Zona/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Zona
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Zona/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
