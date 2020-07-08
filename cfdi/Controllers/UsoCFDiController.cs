using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Services;
using Microsoft.AspNetCore.Mvc;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsoCFDiController : ControllerBase
    {
        // GET: api/<UsoCFDiController>
        [HttpGet]
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
                return BadRequest(res);
            }
            
        }

        // GET api/<UsoCFDiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsoCFDiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsoCFDiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsoCFDiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
