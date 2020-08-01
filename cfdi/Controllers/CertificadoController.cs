using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN, CERTS")]
    public class CertificadoController : ControllerBase
    {
        // GET: api/<Certificado>
        [HttpGet]
        public IActionResult Get(int pagina, int rpp)
        {
            var res = new Dictionary<string, Object>();
            var certService = new CertificadoService();
            try
            {
                res = certService.getCertificados(pagina, rpp);
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

        // GET api/<Certificado>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Certificado>/idRazonSocial
        [HttpPost("{id}")]
        public IActionResult Post(int id, Certificado certificado)
        {
            var res = new Dictionary<string, Object>();
            var certServ = new CertificadoService();
            try
            {
                certServ.insertCertificado(certificado, id);
                res.Add("certificado", certificado);
                return Ok(res);
            }
            catch(Exception e)
            {
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // PUT api/<Certificado>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Certificado>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
