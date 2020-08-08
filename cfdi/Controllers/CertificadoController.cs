using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Models.DTO;
using cfdi.Services;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

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
        public IActionResult Get(int id)
        {
            var res = new Dictionary<string, Object>();
            try
            {
                var certServ = new CertificadoService();
                res.Add("certificado", certServ.GetCertificado(id));
                return Ok(res);
            }
            catch(Exception e)
            {
                if(e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(new { Message = e.Message });
                }
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        //POST api/certificado/carga-cert-archivos/{ID}
        [HttpPost("carga-cert-archivos/{id}")]
        public IActionResult PostCertArchivo(int id, [FromForm]ArchivoCertificado archivos)
        {
            try
            {
                string mensaje = "";
                var certificadoService = new CertificadoService();
                if (!certificadoService.procesarArchivos(id, archivos))
                    mensaje = "Uno o mas archivos no fueron procesados correctamente";
                else
                    mensaje = "Archivo(s) procesado(s) correctamente";
                return Ok(new { Message = mensaje });
            }
            catch(Exception e)
            {
                if(e is InvalidCertificateExtensionType)
                {
                    return BadRequest(new { Message = e.Message});
                }
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // POST api/<Certificado>/{idRazonSocial}
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
                return StatusCode(500, new { Message = e.Message});
            }
        }

        // PUT api/<Certificado>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Certificado certificado)
        {
            var res = new Dictionary<string, Object>();
            var certServ = new CertificadoService();
            try
            {
                certServ.actualizarCertificado(certificado);
                res.Add("certificado", certificado);
                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        [HttpPut("activar/{id}")]
        public IActionResult PutActivarCertificado(int id)
        {
            var certServ = new CertificadoService();
            try
            {
                certServ.ActivarCertificado(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }

        // DELETE api/<Certificado>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var certService = new CertificadoService();
            try
            {
                certService.EliminarCertificado(id);
                return Ok();
            }
            catch
            {
                return StatusCode(500, new { Message = "Error en el servidor" });
            }
        }
    }
}
