using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cfdi.Models;
using cfdi.Services;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cfdiController : ControllerBase
    {
        // GET: api/cfdi
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        // GET: api/cfdi/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/cfdi
        [HttpPost]
        public IActionResult Post(CFDi cfdi)
        {
            XmlBuilderService xmlBuilder = new XmlBuilderService();
            cfdi.xml = xmlBuilder.buildXml(cfdi);
            return Ok(cfdi.xml);
        }

        // PUT: api/cfdi/5
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
