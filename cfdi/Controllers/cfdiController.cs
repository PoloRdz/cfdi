using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cfdi.Models;
using cfdi.Services;
using cfdi.Exceptions;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cfdiController : ControllerBase
    {

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpPost]
        public IActionResult Post(CFDi cfdi)
        {
            var results = new Dictionary<string, Object>();
            try
            {
                TimbradoService timService = new TimbradoService();
                timService.Timbrar(cfdi);
                results.Add("cfdi", cfdi);
                return Ok(results);
            }
            catch (Exception e)
            {
                results.Add("message", e.Message);
                if (e is InvalidRFCException)
                {
                    return NotFound(results);
                }
                if (e is CertificateException || e is InvoiceNumberAvailabilityException)
                {
                    return Conflict(results);
                }
                if (e is InvalidCfdiDataException || e is WebServiceValidationException
                    || e is WebServiceCommunicationException || e is InvalidInvoiceTypeException
                    || e is InvoiceAtZeroException || e is PaymentGreaterThanBalanceException
                    || e is InvalidInvoiceTypeException || e is InvoiceAtZeroException)
                {
                    return BadRequest(results);
                }
                logger.Error(e.Message);
                results.Add("message", "Error en el servidor");
                return BadRequest(results);
            }

        }
    }
}
