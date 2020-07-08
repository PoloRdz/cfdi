using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cfdi.Controllers
{
    [Route("api/razon-social")]
    [ApiController]
    [Authorize]
    public class RazonSocialController : ControllerBase
    {
        // GET: api/RazonSocial
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/RazonSocial/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/RazonSocial
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/RazonSocial/5
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
