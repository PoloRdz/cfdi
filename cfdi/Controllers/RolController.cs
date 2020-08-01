using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Data.DAO;
using cfdi.Exceptions;
using cfdi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        // GET: api/<RolesController>
        [HttpGet]
        public IActionResult Get()
        {
            var res = new Dictionary<string, Object>();
            var rolServ = new RolService();
            try
            {
                res.Add("roles", rolServ.GetRoles());
                return Ok(res);
            }
            catch(Exception e)
            {
                if (e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                else
                {
                    res.Add("message", "Error en el servidor");
                    return StatusCode(500, res);
                }
            }
        }

        // GET api/<RolesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RolesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RolesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
