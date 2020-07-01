using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cfdi.Models;
using cfdi.Services;
using cfdi.Exceptions;
using cfdi.Utils;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cfdiController : ControllerBase
    {
        // GET: api/cfdi
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok();
        //}

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
            try
            {
                TimbradoService timService = new TimbradoService();
                PDFbuilder PDFbuilder = new PDFbuilder();

                PDFbuilder.PDFgenerate(cfdi);

                //timService.Timbrar(cfdi);

                return Ok(value: cfdi);
            }
            catch(InvalidRFCException e)
            {
                return NotFound(e.Message);
            }
            catch(CertificateException e)
            {
                return Conflict(e.Message);
            }
            catch(InvalidCfdiDataException e)
            {
                return BadRequest(e.Message);
            }
            catch(InvoiceNumberAvailabilityException e)
            {
                return Conflict(e.Message);
            }
            catch(WebServiceValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (WebServiceCommunicationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                //Log error
                return BadRequest("Un error inesperado ha sucedido, intentalo mas tarde");
                //return BadRequest(e);
            }

        }

        // PUT: api/cfdi/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
